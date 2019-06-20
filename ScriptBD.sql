-- USER SQL
CREATE USER "PRUEBA" IDENTIFIED BY "PRUEBA123"  ;

-- QUOTAS

-- ROLES
GRANT "CONNECT" TO "PRUEBA" ;

-- SYSTEM PRIVILEGES



CREATE TABLE CONTACTOS
(
IDCONTACTO number,
Nombre VARCHAR2(200), 
Apellido VARCHAR2(200),
PROCESADO VARCHAR2(10) DEFAULT 'false'
)
/

CREATE SEQUENCE SEQCONTACTOS INCREMENT BY 1 START WITH 1 MAXVALUE 99999999999999 MINVALUE 1 NOCACHE;


CREATE OR REPLACE PACKAGE Prueba AS

    PROCEDURE ConsultarContactos (pEstado VARCHAR2, pResultado OUT SYS_REFCURSOR);
    
    PROCEDURE CrearContacto(pNombre VARCHAR2, pApellido VARCHAR2, pResultado OUT SYS_REFCURSOR);
    
    PROCEDURE Procesar;
END;
/

CREATE OR REPLACE PACKAGE BODY Prueba AS

    PROCEDURE ConsultarContactos (pEstado VARCHAR2, pResultado OUT SYS_REFCURSOR) IS
    
    BEGIN
        OPEN pResultado FOR
      SELECT Nombre, 
             Apellido
        FROM CONTACTOS
       WHERE PROCESADO = pEstado;
    
    END;
    
    PROCEDURE CrearContacto(pNombre VARCHAR2, pApellido VARCHAR2, pResultado OUT SYS_REFCURSOR) IS
        lSQLErrCode     NUMBER;
        lSQLErrDesc     VARCHAR2(2048);
    BEGIN
        INSERT INTO CONTACTOS (IDCONTACTO, Nombre, Apellido) VALUES (SEQCONTACTOS.NEXTVAL, pNombre, pApellido);
        commit;
        
        OPEN  pResultado  FOR
      SELECT  0 IdRetorno,
              'Operación exitosa' MensajeRetorno
        FROM DUAL;
        
    EXCEPTION
    WHEN OTHERS THEN
       lsqlerrcode := SQLCODE;
      lsqlerrdesc := substr(sqlerrm,1,2048);


      -- Llenar cursor con info del error
      OPEN pResultado FOR
      SELECT pResultado mensajeretorno
           , lsqlerrcode idretorno
        FROM dual;         
    END;
    
    PROCEDURE Procesar is
    begin
        update CONTACTOS 
           set PROCESADO = 'true'
         where PROCESADO = 'false';
         COMMIT;
    end;
END;
/

GRANT ALL ON Prueba TO PRUEBA ;

BEGIN
    DBMS_SCHEDULER.CREATE_JOB (
            job_name => '"PROCESAR_CONTACTOS"',
            job_type => 'PLSQL_BLOCK',
            job_action => 'BEGIN
Prueba.Procesar;
end;',
            number_of_arguments => 0,
            start_date => NULL,
            repeat_interval => 'FREQ=MINUTELY;BYMINUTE=1',
            end_date => NULL,
            enabled => FALSE,
            auto_drop => FALSE,
            comments => 'PROCESAR CONTACTOS');

         
     
 
    DBMS_SCHEDULER.SET_ATTRIBUTE( 
             name => '"PROCESAR_CONTACTOS"', 
             attribute => 'logging_level', value => DBMS_SCHEDULER.LOGGING_OFF);
      
  
    
    DBMS_SCHEDULER.enable(
             name => '"PROCESAR_CONTACTOS"');
END;
/
