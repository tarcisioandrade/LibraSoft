### Funcionalidades Principais

1. **Gerenciamento de Livros**
	- **Adicionar/Editar/Remover Livros:** Capacidade de adicionar novos livros ao sistema, editar informações de livros existentes e remover livros.
	- **Listar Livros:** Visualizar uma lista de todos os livros disponíveis na biblioteca.
	- **Busca e Filtros:** Permitir a busca por títulos, autores, categorias, etc.

2. **Gerenciamento de Autores e Categorias**
  - **Autores:** Capacidade de adicionar, editar e remover informações de autores.
  - **Categorias:** Capacidade de gerenciar categorias de livros (ex.: Ficção, Não-Ficção, Ciência, etc.)

3. **Gerenciamento de Empréstimos e Bolsa**
    - **Registro de Empréstimos:** Registrar quando um livro é emprestado por um usuário.
    - **Devolução de Livros:** Registrar a devolução dos livros.
      **Histórico de Empréstimos:** Manter um histórico de todos os empréstimos realizados.
    - **Salvar/Remover livros na bolsa/sacola:** O usuário pode salvar ou remover livros no qual tenha interesse em fazer um empréstimo na bolsa/sacola.
    - **Cancelamento automático**: Caso o livro não seja retirado em 24 horas, o pedido deve ser cancelado automaticamente.

4. **Gerenciamento de Usuários**
    - **Autenticação:** Sistema de login e logout para usuários.
    - **Cadastro de Usuários:** Capacidade de novos usuários se cadastrarem.
    - **Perfis de Usuários:** Visualizar e editar informações do perfil do usuário.

5. **Avaliações**
    - **Adicionar:** O usuário pode registrar uma avaliação a um determinado livro.
    - **Curtir**: O usuário pode curtir a avaliação de outro usuário.

6. **Punições**
    - **Suspenso:** Caso o usuário atrase a entrega de um livro uma punição deve ser aplicada impedindo que o usuário efetue novos empréstimos por um tempo determinado.
    - **Banimento:** Caso o usário adquira 3 punições, este usuário deve ser banido permanentemente.

### Regras de Negócio

#### Livros

1. **Cadastro de Livros**
    - Cada livro deve ter um título, autor, categoria, ISBN, data de publicação e número de cópias disponíveis.
    - O ISBN deve ser único para cada livro.
    - O número de cópias disponíveis não pode ser negativo.

2. **Edição de Livros**
    - Todas as informações de um livro podem ser editadas, exceto o ISBN.
    - Ao alterar o número de cópias disponíveis, deve-se ajustar o número de cópias em estoque considerando os empréstimos atuais.

3. **Remoção de Livros**
    - Um livro só pode ser removido se não estiver associado a nenhum empréstimo ativo.
    - Se um livro está associado a empréstimos anteriores (histórico), ele pode ser marcado como "inativo" em vez de ser removido.

#### Autores

1. **Cadastro de Autores**
    - Cada autor deve ter um nome, e opcionalmente uma biografia e data de nascimento.
    - O nome do autor deve ser único no sistema.

2. **Edição de Autores**
    - As informações de um autor podem ser editadas livremente por um administrador.

3. **Remoção de Autores**
    - Um autor só pode ser removido se não estiver associado a nenhum livro ativo.
    - Se o autor está associado a livros, ele pode ser desativado em vez de removido.

#### Categorias

1. **Cadastro de Categorias**
    - Cada categoria deve ter um nome único.
    - As categorias ajudam na organização e filtragem dos livros.

2. **Edição de Categorias**
    - O nome da categoria pode ser editado.

3. **Remoção de Categorias**
    - Uma categoria só pode ser removida se não estiver associada a nenhum livro.
    - Se a categoria está associada a livros, os livros devem ser reclassificados antes de remover a categoria.

#### Usuários

1. **Cadastro de Usuários**
    - Cada usuário deve fornecer nome, email, senha e opcionalmente endereço e telefone.
    - O email deve ser único no sistema.
    - A senha deve atender aos critérios mínimos de segurança (ex.: comprimento mínimo, complexidade).

2. **Edição de Usuários**
    - Os usuários podem editar suas informações pessoais.
    - Os administradores podem alterar o status do usuário (ativo/inativo).

3. **Autenticação**
    - Os usuários devem autenticar-se com email e senha.
    - Implementar mecanismos de recuperação de senha e verificação de email.

#### Empréstimos

1. **Registro de Empréstimos**
    - Um usuário pode emprestar um livro se houver cópias disponíveis.
    - Cada empréstimo deve ter uma data de início e uma data prevista de devolução.
    - O número de empréstimos ativos por usuário pode ser limitado (ex.: máximo de 5 livros emprestados simultaneamente).
    - Um usuário só pode solicitar um empréstimo se não houver nenhuma punição ativa.

2. **Devolução de Livros**
    - Os usuários devem devolver os livros até a data prevista de devolução.
    - Ao devolver um livro, o número de cópias disponíveis é incrementado.
    - Se o livro é devolvido após a data prevista, deve ser aplicada uma punição.


### Relacionamentos Entre Entidades

- **Livro-Autor:** Muitos para um (vários livros podem ter o mesmo autor).
- **Livro-Categoria:** Muitos para um (vários livros podem pertencer à mesma categoria).
- **Livro-Empréstimo:** Muitos para muitos (um livro pode ser emprestado várias vezes, e um empréstimo pode incluir vários livros).
- **Usuário-Empréstimo:** Um para muitos (um usuário pode ter vários empréstimos).

### Considerações Adicionais

- **Auditoria:** Mantenha um registro de todas as operações críticas, como criação, edição e remoção de entidades, para fins de auditoria.
- **Segurança:** Assegure que todas as operações sejam autorizadas, implementando um sistema de permissões baseado em papéis (admin, usuário comum).
- **Performance:** Implemente caching e otimização de consultas para garantir que o sistema possa escalar conforme o número de usuários e livros cresce.
