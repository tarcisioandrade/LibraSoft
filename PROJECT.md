### Funcionalidades Principais

1. **Gerenciamento de Livros**
	- [x] **Adicionar/Editar/Remover Livros:** Capacidade de adicionar novos livros ao sistema, editar informa��es de livros existentes e remover livros.
	- [x] **Listar Livros:** Visualizar uma lista de todos os livros dispon�veis na biblioteca.
	- [x] **Busca e Filtros:** Permitir a busca por t�tulos, autores, g�neros, etc.

1. **Gerenciamento de Autores e Categorias**
    - [x] **Autores:** Capacidade de adicionar, editar e remover informa��es de autores.
    - [x] **Categorias:** Capacidade de gerenciar categorias de livros (ex.: Fic��o, N�o-Fic��o, Ci�ncia, etc.)

2. **Gerenciamento de Empr�stimos**
    - [x]  **Registro de Empr�stimos:** Registrar quando um livro � emprestado por um usu�rio.
    - [x]  **Devolu��o de Livros:** Registrar a devolu��o dos livros.
    - [x] **Hist�rico de Empr�stimos:** Manter um hist�rico de todos os empr�stimos realizados.
    
1. **Gerenciamento de Usu�rios**
    - [x] **Autentica��o:** Sistema de login e logout para usu�rios.
    - [x] **Cadastro de Usu�rios:** Capacidade de novos usu�rios se cadastrarem.
    - [x] **Perfis de Usu�rios:** Visualizar e editar informa��es do perfil do usu�rio.

### Regras de Neg�cio

#### Livros

1. **Cadastro de Livros**
    - Cada livro deve ter um t�tulo, autor, categoria, ISBN, data de publica��o e n�mero de c�pias dispon�veis.
    - O ISBN deve ser �nico para cada livro.
    - O n�mero de c�pias dispon�veis n�o pode ser negativo.

1. **Edi��o de Livros**
    - Todas as informa��es de um livro podem ser editadas, exceto o ISBN.
    - Ao alterar o n�mero de c�pias dispon�veis, deve-se ajustar o n�mero de c�pias em estoque considerando os empr�stimos atuais.

1. **Remo��o de Livros**
    - Um livro s� pode ser removido se n�o estiver associado a nenhum empr�stimo ativo.
    - Se um livro est� associado a empr�stimos anteriores (hist�rico), ele pode ser marcado como "inativo" em vez de ser removido.

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
    - O nome do autor deve ser �nico no sistema.

1. **Edi��o de Autores**
    - As informa��es de um autor podem ser editadas livremente por um administrador.

1. **Remo��o de Autores**
    - Um autor s� pode ser removido se n�o estiver associado a nenhum livro ativo.
    - Se o autor est� associado a livros, ele pode ser desativado em vez de removido.

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
    - Cada categoria deve ter um nome �nico.
    - As categorias ajudam na organiza��o e filtragem dos livros.

1. **Edi��o de Categorias**
    - O nome da categoria pode ser editado.

1. **Remo��o de Categorias**
    - Uma categoria s� pode ser removida se n�o estiver associada a nenhum livro.
    - Se a categoria est� associada a livros, os livros devem ser reclassificados antes de remover a categoria.
```json
{
	"id": "",
	"title": "",
}
```
#### Usu�rios

1. **Cadastro de Usu�rios**
    - Cada usu�rio deve fornecer nome, email, senha e opcionalmente endere�o e telefone.
    - O email deve ser �nico no sistema.
    - A senha deve atender aos crit�rios m�nimos de seguran�a (ex.: comprimento m�nimo, complexidade).

1. **Edi��o de Usu�rios**
    - Os usu�rios podem editar suas informa��es pessoais.
    - Os administradores podem alterar o status do usu�rio (ativo/inativo).

1. **Autentica��o**
    - Os usu�rios devem autenticar-se com email e senha.
    - Implementar mecanismos de recupera��o de senha e verifica��o de email.

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
#### Empr�stimos

1. **Registro de Empr�stimos**
    - Um usu�rio pode emprestar um livro se houver c�pias dispon�veis.
    - Cada empr�stimo deve ter uma data de in�cio e uma data prevista de devolu��o.
    - O n�mero de empr�stimos ativos por usu�rio pode ser limitado (ex.: m�ximo de 5 livros emprestados simultaneamente).

1. **Devolu��o de Livros**
    - Os usu�rios devem devolver os livros at� a data prevista de devolu��o.
    - Ao devolver um livro, o n�mero de c�pias dispon�veis � incrementado.
    - Se o livro � devolvido ap�s a data prevista, pode ser aplicada uma multa.

1. **Renova��o de Empr�stimos**
    - Os usu�rios podem renovar o empr�stimo se o livro n�o estiver reservado por outro usu�rio.
    - O n�mero de renova��es pode ser limitado (ex.: m�ximo de 2 renova��es por empr�stimo).

1. **Reservas de Livros**
    - Os usu�rios podem reservar livros que n�o est�o dispon�veis no momento.
    - Quando uma c�pia reservada � devolvida, o usu�rio que reservou o livro � notificado.
    - As reservas devem ter uma validade ap�s a qual expiram se o livro n�o for retirado.

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

- **Livro-Autor:** Muitos para um (v�rios livros podem ter o mesmo autor).
- **Livro-Categoria:** Muitos para um (v�rios livros podem pertencer � mesma categoria).
- **Livro-Empr�stimo:** Muitos para muitos (um livro pode ser emprestado v�rias vezes, e um empr�stimo pode incluir v�rios livros).
- **Usu�rio-Empr�stimo:** Um para muitos (um usu�rio pode ter v�rios empr�stimos).

### Considera��es Adicionais

- **Auditoria:** Mantenha um registro de todas as opera��es cr�ticas, como cria��o, edi��o e remo��o de entidades, para fins de auditoria.
- **Seguran�a:** Assegure que todas as opera��es sejam autorizadas, implementando um sistema de permiss�es baseado em pap�is (admin, usu�rio comum).
- **Performance:** Implemente caching e otimiza��o de consultas para garantir que o sistema possa escalar conforme o n�mero de usu�rios e livros cresce.

### Endpoints da API

1. **Livros**
    - `GET /api/books` - Listar todos os livros
    - `GET /api/books/{id}` - Obter detalhes de um livro espec�fico
    - `POST /api/books` - Adicionar um novo livro
    - `PUT /api/books/{id}` - Editar um livro existente
    - `DELETE /api/books/{id}` - Remover um livro

1. **Autores**
    - `GET /api/authors` - Listar todos os autores
    - `GET /api/authors/{id}` - Obter detalhes de um autor espec�fico
    - `POST /api/authors` - Adicionar um novo autor
    - `PUT /api/authors/{id}` - Editar um autor existente
    - `DELETE /api/authors/{id}` - Remover um autor

2. **Empr�stimos**
    - `GET /api/loans` - Listar todos os empr�stimos
    - `POST /api/loans` - Registrar um novo empr�stimo
    - `PUT /api/loans/{id}/return` - Registrar a devolu��o de um livro