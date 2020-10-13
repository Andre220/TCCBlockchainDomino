# TCCBlockchainDomino

  Trabalho de conclusão do curso de Ciências da Computação na Unicarioca, desenvolvido por André Felipe dos Santos e orientado por Anderson Fernandes Pereira dos Santos.
  
  #### Introdução:
  Esta pesquisa busca apresentar uma alternativa ao modelo tradicional
Cliente-Servidor com um **servidor autoritativo** empregado em jogos online que apesar de seus múltiplos
benefícios, ainda possui pontos negativos como:
1. A necessidade de investimento monetário por parte dos desenvolvedores para manter os servidores
2. A possibilidade dos jogadores perderem todos os seus ativos adquiridos caso os
servidores parem. 

  Para tal, foi criado um jogo que opera em conjunto com uma
blockchain desenvolvida em C#, a fim de manter um registro das jogadas feitas. 
Verificou-se que é possível manter um jogo persistido em uma blockchain, gerando transações a cada
jogada e tendo seus blocos minerados pelos jogadores, sem gerar custos a equipe que desenvolveu o jogo. 
Conclui-se que esta é uma prática promissora para se ter jogos online com menos custo e mais seguros.
Entretanto, mais pesquisas na área são necessárias antes que seja possível aplicar este conceito em produtos reais.
  
  
  #### Plataforma
  Microsoft Windows
  
  #### Softwares/Tecnologias/bibliotecas usadas:
  1. C#
  2. Unity3D
  3. Unet
  
  #### Orientações para testar a aplicação:
  1. Para facilitar a testagem, efetuei duas builds, localizadas no diretório "TCCBlockchainDomino\Build\BuildA" e "TCCBlockchainDomino\Build\BuildB".
  2. Entrar no diretório de cada build e executar o arquivo "TCCDomino" em cada um, abrindo **DUAS** intâncias do jogo.
  3. Em uma instância, preencher o campo **porta** e **nickname** como na imagem *3.1*: </br>
  - 3.1 ![Campos de login preenchidos](https://github.com/Andre220/TCCBlockchainDomino/blob/master/Documentação/Imagens/1-Login.png)
  4. Repetir o processo 3 na outra instância aberta, lembrando que o campo **porta** precisa ter um valor diferente em cada instância.
  5. Na tela principal (imagem *5.1*) basta inserir a porta do adversário que o jogo se inicia em ambos os clientes.
  Repare que no display ao lado direito, ao se arrastar para baixo algumas vezes, é possível ver as informações do bloco gênesis:</br>
  - 5.1 ![Preenchimento da porta do adversário](https://github.com/Andre220/TCCBlockchainDomino/blob/master/Documentação/Imagens/2-TelaPrincipal.png)
  6. Na tela de jogo (imagem *6.1*), ao se clicar no botão "transações", um display é exibido com todas as transações (jogadas) feitas na partida (imagem *6.2*):</br>
  - 6.1 ![Tela de jogo](https://github.com/Andre220/TCCBlockchainDomino/blob/master/Documentação/Imagens/3-GamePlay.png)
  - 6.2 ![Tela de transações](https://github.com/Andre220/TCCBlockchainDomino/blob/master/Documentação/Imagens/4-GamePlayTransacoes.png)
  7. Quando um dos clientes clica no botão "EndGame", ele minera o bloco com as transações feitas durante partida e é direcionado para a tela principal, como exibido na imagem *7.1*: </br>
  - 7.1 ![Após minerar o bloco](https://github.com/Andre220/TCCBlockchainDomino/blob/master/Documentação/Imagens/5-TelaPrincipalBlocos.png)

  #### Bugs conhecidos:
  1. Durante a partida de dominó, no menu que exibe as transações, as informações de cada transação expecífica não são exibidas na lista.
  2. As N instâncias da aplicação que estiverem rodando não possuem o mesmo hash para o bloco gênesis. É necessário desenvolver uma solução
  3. Ao conluir um jogo, o nó que encerrou o jogo executa a mineração do bloco com as transações do jogo. No entando, após concluir a mineração, o nó **NÃO** envia o bloco minerado para o outro nó.
  
  #### Lista de melhorias:
  - [ ] Corrigir a view das transações, a fim de exibir os dados da transação no jogo, e não apenas no BlockchainJson
  - [ ] Fazer com que o BlockchainExportImporter.cs gere um arquivo "Blockchain.json" e não uma rquivo "BlockchainJson.txt"
  - [ ] Inserir uma lógica que permita um nó A perguntar se um nó B possui um blockchainJson.txt e responda o nó A com esse arquivo , para evitar que cada cliente gere um bloco gênesis diferente
  - [ ] Substituir o "módulo" Networking (todos os arquivos dentro da pasta networking, excetuando as interfaces) que utiliza [Unet, que está sendo abandonado](https://blogs.unity3d.com/2018/08/02/evolving-multiplayer-games-beyond-unet/?_ga=2.96933339.555628980.1598910975-436933837.1598910975) como descrito [aqui](https://support.unity3d.com/hc/en-us/articles/360001252086-UNet-Deprecation-FAQ?_ga=2.3036080.1456827226.1598913940-1273657642.1598913940). Estou avaliando o [Mirror](https://mirror-networking.com/docs/) por ser gratuito (uma das premissas do projeto).
  - [ ] Melhorar a forma como os componentes interagem - a implementação da Interface do cliente é feita em um singleton, que toda a aplicação consome. Pretendo usar [Injeção de Dependência com Zenject](https://github.com/modesttree/Zenject) para eliminar esse problema.
  - [ ] Atualmente, a implementação da Unet opera apenas localmente, utilizando as **PORTAS** de rede como identificador de outros jogadores. Isso se deve a necessidade de fazer [*port forwarding* e configurar o *NAT*](https://pplware.sapo.pt/tutoriais/networking/sabe-port-forwarding-qual-utilizacao/) para que a aplicação funcionasse através da internet. E estas ações estavam fora do escopo desta prova de conceito, em especial por questões de tempo.
  - [ ] Inserir uma espécie de "ThreeWayHandshake" a fim de permitir que os clientes escolham se querem aceitar uma partida ou não. Atualmente, ao receber uma requisição de conexão, ambos os clientes entram no jogo.
  - [ ] O ato de minerar um bloco deve ser feito assim que as peças de um dos jogadores for erada, e não quando um dos jogadores clica no botão "EndGame".
  
  
  #### Créditos:
  1. André Felipe dos Santos (Desenvolvedor)

  #### Links:
  1. [Versão final da parte escrita](https://drive.google.com/file/d/1esuhM6GoZV3GJIRjhBi159EF-h4V8inh/view?usp=sharing)
  2. [BuildA e BuildB da aplicação](https://github.com/Andre220/TCCBlockchainDomino/tree/master/Build)
