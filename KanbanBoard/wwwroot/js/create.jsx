class CardForm extends React.Component {

    constructor(props, context) {
        super(props, context);
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
        if (!CardTitle || !CardDescription) {
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
                        placeholder="Title"
                        value={this.state.title}
                        onChange={this.onTitleChange} />
                </p>
                <p>
                    <input type="text"
                        placeholder="Description"
                        value={this.state.description}
                        onChange={this.onDescriptionChange} />
                </p>
                <input type="submit" value="Add" />
            </form>
        );
    }
}


class TaskDashboard extends React.Component {

    constructor(props) {
        super(props);
        this.onAddCard = this.onAddCard.bind(this);
    }
    onAddCard(card) {
        if (card) {

            var data = JSON.stringify({ "title": card.title, "description": card.description });
            var xhr = new XMLHttpRequest();

            xhr.open("post", this.props.apiUrl + "/postcard", true);
            xhr.setRequestHeader("Content-type", "application/json");
            //xhr.onload = function () {
            //    if (xhr.status == 200) {
            //        this.context.router.push(this.props.apiUrl);
            //    }
            //}.bind(this);
            xhr.send(data);
        }
    }
    render() {
        var remove = this.onRemoveCard;
        return <div className="cardForm">
            <h1>Create Task</h1>
            <CardForm onCardSubmit={this.onAddCard} />
            <a href={this.props.apiUrl}>Go to Dashboard</a>
        </div>;
    }
}

ReactDOM.render(
    <TaskDashboard apiUrl="/dashboard" />,
    document.getElementById("content")
);