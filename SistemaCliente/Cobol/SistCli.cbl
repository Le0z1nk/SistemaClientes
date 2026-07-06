       IDENTIFICATION DIVISION.
       PROGRAM-ID. sistcli.
       ENVIRONMENT DIVISION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
           SELECT OPTIONAL CLIENTES ASSIGN TO "clientes.dat"
           ORGANIZATION IS INDEXED
           ACCESS MODE IS RANDOM
           RECORD KEY IS REQ-CODIGO
           FILE STATUS IS WS-FS.
           SELECT RESPOSTA-FILE ASSIGN TO "resposta.txt"
           ORGANIZATION IS LINE SEQUENTIAL.
       DATA DIVISION.
       FILE SECTION.
       FD  CLIENTES.
       01  REG-CLIENTE.
           05 REQ-CODIGO    PIC X(05).
           05 REQ-NOME      PIC X(50).
           05 REQ-TELEFONE  PIC X(15).
           05 REQ-EMAIL     PIC X(40).
       FD  RESPOSTA-FILE.
       01  REG-RESPOSTA     PIC X(110).
       WORKING-STORAGE SECTION.
       01  WS-FS             PIC X(02).
       01  WS-ENCONTRADO     PIC X(01) VALUE "N".

       01  LK-PARAMETROS.
           05 LK-OPERACAO   PIC X(03).
           05 LK-CODIGO     PIC X(05).
           05 LK-NOME       PIC X(50).
           05 LK-TELEFONE   PIC X(15).
           05 LK-EMAIL      PIC X(40).
       PROCEDURE DIVISION.
       MAIN-PROCEDURE.
           ACCEPT LK-PARAMETROS FROM COMMAND-LINE.
           DISPLAY "DEBUG OP: [" LK-OPERACAO "]"
           DISPLAY "DEBUG COD: [" LK-CODIGO "]"
           IF LK-OPERACAO = "CON"
               PERFORM CONSULTAR-CLIENTE
           ELSE IF LK-OPERACAO = "ALT"
               PERFORM ALTERAR-CLIENTE
           ELSE IF LK-OPERACAO = "CAD"
               PERFORM CADASTRAR-CLIENTE
           END-IF.
           STOP RUN.

       CONSULTAR-CLIENTE.
           OPEN INPUT CLIENTES
           OPEN OUTPUT RESPOSTA-FILE
           *> Move a chave buscada para o registro do arquivo
           MOVE LK-CODIGO TO REQ-CODIGO
           READ CLIENTES
               INVALID KEY
                   MOVE "NOT_FOUND" TO REG-RESPOSTA
               NOT INVALID KEY
                   MOVE REG-CLIENTE TO REG-RESPOSTA
           END-READ
           WRITE REG-RESPOSTA
           CLOSE CLIENTES
           CLOSE RESPOSTA-FILE.

       ALTERAR-CLIENTE.
           OPEN I-O CLIENTES
           OPEN OUTPUT RESPOSTA-FILE
           MOVE LK-CODIGO TO REQ-CODIGO
           READ CLIENTES
               INVALID KEY
                   MOVE "NOT_FOUND" TO REG-RESPOSTA
               NOT INVALID KEY
                   MOVE LK-TELEFONE TO REQ-TELEFONE
                   MOVE LK-EMAIL    TO REQ-EMAIL
                   REWRITE REG-CLIENTE
                   WRITE REG-RESPOSTA FROM REG-CLIENTE
           END-READ
           CLOSE CLIENTES
           CLOSE RESPOSTA-FILE.
       CADASTRAR-CLIENTE.
           OPEN I-O CLIENTES
           OPEN OUTPUT RESPOSTA-FILE

           IF WS-FS NOT = "00" AND WS-FS NOT = "05"
               MOVE "FILE_OPEN_ERROR" TO REG-RESPOSTA
               WRITE REG-RESPOSTA
           ELSE
               MOVE LK-CODIGO    TO REQ-CODIGO
               MOVE LK-NOME TO REQ-NOME
               MOVE LK-TELEFONE  TO REQ-TELEFONE
               MOVE LK-EMAIL    TO REQ-EMAIL
               WRITE REG-CLIENTE
                   INVALID KEY
                       MOVE "DUPLICATE_KEY" TO REG-RESPOSTA
                   NOT INVALID KEY
                       MOVE REG-CLIENTE TO REG-RESPOSTA
               END-WRITE
               WRITE REG-RESPOSTA
           END-IF.
           CLOSE CLIENTES
           CLOSE RESPOSTA-FILE.
