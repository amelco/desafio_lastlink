# Desafio

Aqui é apresentado o código fonte do desafio.

## Decisões arquiteturais
Conforme solicitado pelo desafio, um padrão de arquitetura limpa foi utilizado, mais especificamente a arquitetura [Onion](https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/).

Inspirada nessa arquitetura, nossa solução foi dividida em três camadas:
- Api
- Infra
- Core

A camada `Core` é a central, que define o estado e o comportamento das entidades. Ela não tem dependências externas. A camada de `Infra`, por sua vez, é a que gerencia entradas e saídas de tudo que é ligado ao exterior da solução (em nosso caso, banco de dados e sistema de filas). Finalmente, a camada `Api` é responsável por expor a interface de nossa solução aos seus consumidores, através dos endpoints da api RESTful.

## Decisões de implementação

Decidimos implementar cada camada como sendo um projeto à parte em nossa solução. Essa escolha foi feita devido aos seguintes fatores:
- Separação de responsabilidade: O propósto de cada camada é bem definida, dificultando a implementação de código "espaguete".
- Baixo acoplamento: As dependências são explicitamente declaradas através de interfaces. Facilta a substituição de implementações diferentes.
- Manutenabilidade: Manutenção de uma camada não afeta a outra se os contratos (assinatura das interfaces) são mantidas.

Como tudo na engenharia de software, o uso desses benefícios implicam que outras caraterísticas sofrerão desvantagens, sendo elas:
- Maior tempo de compilação por se ter vários projetos
- Código ineretemente mais complexo
- Maior dificuldade de deploy

### Sistema de fila
O RabbitMQ foi utilizado como o message broker responsável pelo gerenciamento das filas da aplicação. Tomei a decisão de fazer o `Publisher` e o `Consumer` na mesma aplicação devido a maior facilidade de desenvolvimento, de implementação do deploy e à quantidade de tempo que eu tinha disponível. De outra forma, eu teria que implementar e configurar duas aplicações distintas que precisariam se comunicar através do rabbitmq server.

Entretanto, uma implementação igual a essa em ambiente de produção pode acarretar problemas de escalabilidade, já que os publishers e os consumers podem apresentar cargas completamente distintas, a depender do caso de uso. Uma sobrecarga em apenas um impacta diretamente o outro já que estão na mesma aplicação, e isso deve ser evitado.

### Banco de dados
Para a implementação da persistência dos dados foi feita utilizando o MS SqlServer. As duas tabelas foram criadas (`Products` e `ProductEvents`), com um relacionamento um-para-muitos, de forma que um produto pode ter muitos eventos atrelados a ele.

### Bibliotecas externas
As principais bibliotecas utilizadas foram:
- Entity Framework: para facilitar a comunicação da aplicação com o banco de dados
- Mapster: para facilitar o mapeamento das entidades com a sua apresentação ao mundo externo (dto's)
- Xunit: para realizar os testes unitarios
- Swashbuckle: para disponibilzar os endpoints de maneira de fácil utilização (swagger)


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

## Testes unitários
Execute:
```bash
dotnet test
```

## Observações

- Foi disponibilizado um endpoint adicional `/products/events` somente para facilitar a visualização dos registros tabela de auditoria `ProductEvents`..