### Funcionalidades Principais

1. **Gerenciamento de Livros**
	- [x] **Adicionar/Editar/Remover Livros:** Capacidade de adicionar novos livros ao sistema, editar informações de livros existentes e remover livros.
	- [x] **Listar Livros:** Visualizar uma lista de todos os livros disponíveis na biblioteca.
	- [x] **Busca e Filtros:** Permitir a busca por títulos, autores, gêneros, etc.

1. **Gerenciamento de Autores e Categorias**
    - [x] **Autores:** Capacidade de adicionar, editar e remover informações de autores.
    - [x] **Categorias:** Capacidade de gerenciar categorias de livros (ex.: Ficção, Não-Ficção, Ciência, etc.)

2. **Gerenciamento de Empréstimos**
    - [x]  **Registro de Empréstimos:** Registrar quando um livro é emprestado por um usuário.
    - [x]  **Devolução de Livros:** Registrar a devolução dos livros.
    - [x] **Histórico de Empréstimos:** Manter um histórico de todos os empréstimos realizados.
    
1. **Gerenciamento de Usuários**
    - [x] **Autenticação:** Sistema de login e logout para usuários.
    - [x] **Cadastro de Usuários:** Capacidade de novos usuários se cadastrarem.
    - [x] **Perfis de Usuários:** Visualizar e editar informações do perfil do usuário.

### Regras de Negócio

#### Livros

1. **Cadastro de Livros**
    - Cada livro deve ter um título, autor, categoria, ISBN, data de publicação e número de cópias disponíveis.
    - O ISBN deve ser único para cada livro.
    - O número de cópias disponíveis não pode ser negativo.

1. **Edição de Livros**
    - Todas as informações de um livro podem ser editadas, exceto o ISBN.
    - Ao alterar o número de cópias disponíveis, deve-se ajustar o número de cópias em estoque considerando os empréstimos atuais.

1. **Remoção de Livros**
    - Um livro só pode ser removido se não estiver associado a nenhum empréstimo ativo.
    - Se um livro está associado a empréstimos anteriores (histórico), ele pode ser marcado como "inativo" em vez de ser removido.

```json
{
	"id" : "1as-12-asa-12as",
	"title":"",
	"author": "",
	"publisher": "",
	"isbn": "21212122",
	"publication_at": "",
	"category": 0,
	"copies_disponible": 50,
	"status": "inative"
}
```
#### Autores

1. **Cadastro de Autores**
    - Cada autor deve ter um nome, e opcionalmente uma biografia e data de nascimento.
    - O nome do autor deve ser único no sistema.

1. **Edição de Autores**
    - As informações de um autor podem ser editadas livremente por um administrador.

1. **Remoção de Autores**
    - Um autor só pode ser removido se não estiver associado a nenhum livro ativo.
    - Se o autor está associado a livros, ele pode ser desativado em vez de removido.

```json
{
	"id": "",
	"name": "",
	"biography": "",
	"date_birth": "",
	"status": "inactive"
}
```
#### Categorias

1. **Cadastro de Categorias**
    - Cada categoria deve ter um nome único.
    - As categorias ajudam na organização e filtragem dos livros.

1. **Edição de Categorias**
    - O nome da categoria pode ser editado.

1. **Remoção de Categorias**
    - Uma categoria só pode ser removida se não estiver associada a nenhum livro.
    - Se a categoria está associada a livros, os livros devem ser reclassificados antes de remover a categoria.
```json
{
	"id": "",
	"title": "",
}
```
#### Usuários

1. **Cadastro de Usuários**
    - Cada usuário deve fornecer nome, email, senha e opcionalmente endereço e telefone.
    - O email deve ser único no sistema.
    - A senha deve atender aos critérios mínimos de segurança (ex.: comprimento mínimo, complexidade).

1. **Edição de Usuários**
    - Os usuários podem editar suas informações pessoais.
    - Os administradores podem alterar o status do usuário (ativo/inativo).

1. **Autenticação**
    - Os usuários devem autenticar-se com email e senha.
    - Implementar mecanismos de recuperação de senha e verificação de email.

```json
{
	"id": "",
	"name": "",
	"email": "",
	"telephone": "",
	"password": "",
	"address": {
		"city": "",
		"street": "",
		"state": "",
		"country": "",
		"zipcode": ""
	},
	"status": "",
	"role": "",
	"loans": ""
}
```
#### Empréstimos

1. **Registro de Empréstimos**
    - Um usuário pode emprestar um livro se houver cópias disponíveis.
    - Cada empréstimo deve ter uma data de início e uma data prevista de devolução.
    - O número de empréstimos ativos por usuário pode ser limitado (ex.: máximo de 5 livros emprestados simultaneamente).

1. **Devolução de Livros**
    - Os usuários devem devolver os livros até a data prevista de devolução.
    - Ao devolver um livro, o número de cópias disponíveis é incrementado.
    - Se o livro é devolvido após a data prevista, pode ser aplicada uma multa.

1. **Renovação de Empréstimos**
    - Os usuários podem renovar o empréstimo se o livro não estiver reservado por outro usuário.
    - O número de renovações pode ser limitado (ex.: máximo de 2 renovações por empréstimo).

1. **Reservas de Livros**
    - Os usuários podem reservar livros que não estão disponíveis no momento.
    - Quando uma cópia reservada é devolvida, o usuário que reservou o livro é notificado.
    - As reservas devem ter uma validade após a qual expiram se o livro não for retirado.

```json
{
	"id": "",
	"user_id": "",
	"book_id": "",
	"rent_date": "",
	"return_date": "",
	"status": "Returned" // Returned | Rental | Pending
 }
```
### Relacionamentos Entre Entidades

- **Livro-Autor:** Muitos para um (vários livros podem ter o mesmo autor).
- **Livro-Categoria:** Muitos para um (vários livros podem pertencer à mesma categoria).
- **Livro-Empréstimo:** Muitos para muitos (um livro pode ser emprestado várias vezes, e um empréstimo pode incluir vários livros).
- **Usuário-Empréstimo:** Um para muitos (um usuário pode ter vários empréstimos).

### Considerações Adicionais

- **Auditoria:** Mantenha um registro de todas as operações críticas, como criação, edição e remoção de entidades, para fins de auditoria.
- **Segurança:** Assegure que todas as operações sejam autorizadas, implementando um sistema de permissões baseado em papéis (admin, usuário comum).
- **Performance:** Implemente caching e otimização de consultas para garantir que o sistema possa escalar conforme o número de usuários e livros cresce.

### Endpoints da API

1. **Livros**
    - `GET /api/books` - Listar todos os livros
    - `GET /api/books/{id}` - Obter detalhes de um livro específico
    - `POST /api/books` - Adicionar um novo livro
    - `PUT /api/books/{id}` - Editar um livro existente
    - `DELETE /api/books/{id}` - Remover um livro

1. **Autores**
    - `GET /api/authors` - Listar todos os autores
    - `GET /api/authors/{id}` - Obter detalhes de um autor específico
    - `POST /api/authors` - Adicionar um novo autor
    - `PUT /api/authors/{id}` - Editar um autor existente
    - `DELETE /api/authors/{id}` - Remover um autor

2. **Empréstimos**
    - `GET /api/loans` - Listar todos os empréstimos
    - `POST /api/loans` - Registrar um novo empréstimo
    - `PUT /api/loans/{id}/return` - Registrar a devolução de um livro