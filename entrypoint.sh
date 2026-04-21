#!/bin/bash
echo "Aguardando o banco de dados iniciar."
sleep 120
echo "Iniciando a aplicação."
exec "$@"
