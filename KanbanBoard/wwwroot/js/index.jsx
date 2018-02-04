class TaskCard extends React.Component {

    constructor(props) {
        super(props);
        this.state = { data: props.card };
        this.onClick = this.onClick.bind(this);
    }
    onClick(e) {
        this.props.onRemove(this.state.data);
    }
    render() {
        return <div className={this.state.data.state.name}>
            <p><b>{this.state.data.title}</b></p>
            <p>{this.state.data.description}</p>
            <p><button onClick={this.onClick}>Delete</button></p>
        </div>;
    }
}

class CardForm extends React.Component {

    constructor(props) {
        super(props);
        this.state = { title: "", description: "" };

        this.onSubmit = this.onSubmit.bind(this);
        this.onTitleChange = this.onTitleChange.bind(this);
        this.onDescriptionChange = this.onDescriptionChange.bind(this);
    }
    onTitleChange(e) {
        this.setState({ title: e.target.value });
    }
    onDescriptionChange(e) {
        this.setState({ description: e.target.value });
    }
    onSubmit(e) {
        e.preventDefault();
        var CardTitle = this.state.title.trim();
        var CardDescription = this.state.description;
        if (!CardName || CardPrice <= 0) {
            return;
        }
        this.props.onCardSubmit({ title: CardTitle, description: CardDescription });
        this.setState({ title: "", description: "" });
    }
    render() {
        return (
            <form onSubmit={this.onSubmit}>
                <p>
                    <input type="text"
                        placeholder="Модель телефона"
                        value={this.state.name}
                        onChange={this.onNameChange} />
                </p>
                <p>
                    <input type="number"
                        placeholder="Цена"
                        value={this.state.price}
                        onChange={this.onPriceChange} />
                </p>
                <input type="submit" value="Сохранить" />
            </form>
        );
    }
}


class TaskDashboard extends React.Component {

    constructor(props) {
        super(props);
        this.state = { card: [] };

        this.onAddCard = this.onAddCard.bind(this);
        this.onRemoveCard = this.onRemoveCard.bind(this);
    }
    // загрузка данных
    loadData() {
        var xhr = new XMLHttpRequest();
        xhr.open("get", this.props.apiUrl + "/getall", true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.setState({ card: data });
        }.bind(this);
        xhr.send();
    }
    componentDidMount() {
        this.loadData();
    }
    // добавление объекта
    onAddCard(card) {
        if (card) {

            var data = JSON.stringify({ "title": card.title, "description": card.description });
            var xhr = new XMLHttpRequest();

            xhr.open("post", this.props.apiUrl + "/postcard", true);
            xhr.setRequestHeader("Content-type", "application/json");
            xhr.onload = function () {
                if (xhr.status == 200) {
                    this.loadData();
                }
            }.bind(this);
            xhr.send(data);
        }
    }
    // удаление объекта
    onRemoveCard(card) {

        if (card) {
            var url = this.props.apiUrl + "/deletecard/" + card.id;

            var xhr = new XMLHttpRequest();
            xhr.open("delete", url, true);
            xhr.setRequestHeader("Content-Type", "application/json");
            xhr.onload = function () {
                if (xhr.status == 200) {
                    this.loadData();
                }
            }.bind(this);
            xhr.send();
        }
    }
    //<CardForm onCardSubmit={this.onAddCard} />
    render() {
        var remove = this.onRemoveCard;
        return <div className="dashboard">
            <h1>Dashboard</h1>
            <a href={this.props.apiUrl + "/create"}>Add task</a>

            <ul>
                <li>To Do</li>
                <li>In Progress</li>
                <li>Done</li>
            </ul>
            <div className="taskList">
                {
                    this.state.card.map(function (card) {

                        return <TaskCard key={card.id} card={card} onRemove={remove}/>;
                    })
                }
            </div>
        </div>;
    }
}

ReactDOM.render(
    <TaskDashboard apiUrl="/dashboard" />,
    document.getElementById("content")
);