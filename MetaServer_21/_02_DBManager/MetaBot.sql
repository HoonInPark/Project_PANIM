--SYSTEM 계정으로 처리하는 부분 START
DROP USER metabot CASCADE;

CREATE USER metabot IDENTIFIED BY metabot DEFAULT TABLESPACE users TEMPORARY TABLESPACE temp PROFILE DEFAULT;

GRANT CONNECT, RESOURCE TO metabot ;
GRANT CREATE VIEW, CREATE SYNONYM TO metabot;

ALTER USER metabot ACCOUNT UNLOCK;

conn metabot/metabot;
--SYSTEM 계정으로 처리하는 부분 END
--metabot START
--metabot 접속했으므로 metabot 사용자의 테이블이 생성
DROP SEQUENCE log_sequence;

DROP TABLE log;
DROP TABLE member;
DROP TABLE gameroom;

CREATE SEQUENCE log_sequence
  START WITH 6
  INCREMENT BY 1
  NOMAXVALUE
  NOMINVALUE
  NOCYCLE
  CACHE 20;

CREATE TABLE gameroom (
  gameroom_id   VARCHAR2(100),
  gameroom_name VARCHAR2(100)
);
CREATE TABLE member (
  member_id     VARCHAR2(100),
  member_pass   VARCHAR2(100),
  member_name   VARCHAR2(100),
  member_age    VARCHAR2(3),
  member_job    VARCHAR2(100),
  member_phone  VARCHAR2(40),
  gameroom_id   VARCHAR2(100)
);
CREATE TABLE log (
  log_seq   NUMBER,
  log_time  DATE default SYSDATE,
  log_ip    VARCHAR2(20),
  log_port  VARCHAR2(5),
  log_info  VARCHAR2(1000),
  member_id VARCHAR2(100)
);

INSERT INTO gameroom VALUES('WORLD_01', 'ABC');
INSERT INTO gameroom VALUES('WORLD_02', 'Korea');
INSERT INTO gameroom VALUES('WORLD_03', 'Japan');
INSERT INTO gameroom VALUES('WORLD_04', 'China');
INSERT INTO gameroom VALUES('WORLD_05', 'Zepeto');
COMMIT;

INSERT INTO member VALUES('asdf', '1234', '홍길동', '24', '활빈당', '010-1111-1111', 'WORLD_01');
INSERT INTO member VALUES('qwer', '1234', '임꺽정', '34', '의적', '010-2222-2222', 'WORLD_02');
INSERT INTO member VALUES('zxcv', '1234', '장길산', '44', '광대', '010-3333-3333', 'WORLD_03');
INSERT INTO member VALUES('aaaa', '1234', '차돌바위', '21', '나무꾼', '010-4444-5555', 'WORLD_04');
INSERT INTO member VALUES('bbbb', '1234', '일지매', '31', '댄서', '010-5555-4444', 'WORLD_05');
INSERT INTO member VALUES('cccc', '1234', 'Albert', '41', 'Developer', '010-6666-7777', 'WORLD_05');
INSERT INTO member VALUES('dddd', '1234', 'Alex', '55', 'Knight', '010-7777-6666', 'WORLD_04');
INSERT INTO member VALUES('eeee', '1234', 'James', '25', 'Writer', '010-8888-9999', 'WORLD_03');
INSERT INTO member VALUES('ffff', '1234', 'Henry', '26', 'Teacher', '010-9999-8888', 'WORLD_02');
INSERT INTO member VALUES('gggg', '1234', 'Brown', '29', 'Boxer', '010-0000-1111', 'WORLD_01');
COMMIT;

ALTER SESSION SET nls_date_format='YYYY-MM-DD:HH24:MI:SS';
INSERT INTO log VALUES(1, '2023-04-20:11:59:24', '192.168.10.21', '9876', 'WORLD_02 Enter', 'asdf');
INSERT INTO log VALUES(2, '2023-04-20:13:29:45', '192.168.10.25', '9111', 'WORLD_02 Enter', 'aaaa');
INSERT INTO log VALUES(3, '2023-04-20:15:32:11', '192.168.10.11', '10980', 'WORLD_03 Enter', 'cccc');   
INSERT INTO log VALUES(4, '2023-04-21:10:10:20', '192.168.10.25', '9111', 'aaaa killed asdf in WORLD_02', 'aaaa');
INSERT INTO log VALUES(5, '2023-04-21:12:59:14', '192.168.10.25', '9111', 'WORLD_02 Leave', 'aaaa');
COMMIT;

-- PRIMARY KEY 설정
ALTER TABLE gameroom
 ADD CONSTRAINT gameroom_gameroom_id_pk PRIMARY KEY(gameroom_id);

ALTER TABLE member
 ADD CONSTRAINT member_member_id_pk PRIMARY KEY(member_id);

ALTER TABLE log
 ADD CONSTRAINT log_log_seq_pk PRIMARY KEY(log_seq);

-- FOREIGN KEY 설정
ALTER TABLE member
 ADD CONSTRAINT member_gameroom_id_fk FOREIGN KEY(gameroom_id) 
 REFERENCES gameroom(gameroom_id);

ALTER TABLE log 
 ADD CONSTRAINT member_member_id_fk FOREIGN KEY(member_id)
 REFERENCES member(member_id);


-- private OracleConnectionPool(string ip, int port, string service_name, string id, string password)
-- {
--     this.ip = ip;
--     this.port = port;
--     this.service_name = service_name;
--     this.id = id;
--     this.password = password;

--     this.OracleConnInfo = $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={ip})(PORT={port})))" +
--         $"(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={service_name}))); User Id = {id}; Password = {password};";
-- }
