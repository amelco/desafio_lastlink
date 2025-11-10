# Desafio

Aqui é apresentado o código fonte do desafio.

## Compilando
Após descompactar o arquivo zip, dentro do diretorio `desafio/`, execute:
```bash
docker compose up -d
```

Isso irá baixar as imagens docker necessárias (mssql server e rabbitmq server), compilar a versão de Release
do projeto e executá-la em um container.

## Executando
Após a compilação, a web api estará disponível em `http://localhost`.

Apenas para esse desafio, o swagger está disponível em `http://localhost/swagger/index.html` (não é boa prática expor o swagger em ambientes de procução).

## Observações

- Foi disponibilizado um endpoint adicional `/products/events` somente para facilitar a visualização dos registros tabela de auditoria `ProductEvents`..