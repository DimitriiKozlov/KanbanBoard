class TaskCard extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            data: props.card,
            isUpdated: false,
            updateTitle: props.card.title,
            updateDescription: props.card.description,
            //tdCard: props.card.filter(c => c.state.name == "ToDo"),
            //ipCard: props.card.filter(c => c.state.name == "InProgress"),
            //dnCard: props.card.filter(c => c.state.name == "Done"),
        };
        this.onClickDelete = this.onClickDelete.bind(this);
        this.onClickUpdate = this.onClickUpdate.bind(this);
        this.onClickSave = this.onClickSave.bind(this);
        this.onTitleChange = this.onTitleChange.bind(this);
        this.onDescriptionChange = this.onDescriptionChange.bind(this);
        this.onClickChangeStatus = this.onClickChangeStatus.bind(this);
    }
    onClickDelete(e) {
        this.props.onRemove(this.state.data);
    }
    onClickUpdate(e) {
        this.setState({ isUpdated: true });
    }
    onClickSave(e) {
        this.setState({ isUpdated: false });
        this.props.onUpdate(this.state.data.id, this.state.updateTitle, this.state.updateDescription, this);
    }
    onTitleChange(e) {
        this.setState({ updateTitle: e.target.value });
    }
    onDescriptionChange(e) {
        this.setState({ updateDescription: e.target.value });
    }
    onClickChangeStatus(e) {
        this.props.onChangeStatus(this.state.data, this);
    }
    render() {
        if (this.state.isUpdated) {
            return <div className={this.state.data.state.name + " "}>
                       <div className="card-header text-center">
                           <div className="btn-group-justified" role="group">
                        <button className="btn btn-outline-light" onClick={this.onClickDelete}>Delete</button>
                        <button className="btn btn-outline-light" onClick={this.onClickSave}>Save</button>
                               <button className="btn btn-outline-light" onClick={this.onClickChangeStatus}>=></button>
                           </div>
                       </div>
                       <div className="card-body text-center">
                    <input class="input-group-text bg-secondary text-white" type="text"
                                  value={this.state.updateTitle}
                                  onChange={this.onTitleChange} />
                    <input class="input-group-text bg-secondary text-white" type="text" type="text"
                                  value={this.state.updateDescription}
                                  onChange={this.onDescriptionChange} />
                        </div>
                   </div>;
        }
        if (this.state.data.state.name == "Done") {
            return <div className={this.state.data.state.name + " "}>
                       <div className="card-header text-center">
                           <div className="btn-group-justified" role="group">
                        <button className="btn btn-outline-light text-right" onClick={this.onClickChangeStatus}>=></button>
                           </div>
                       </div>
                       <h5 className="card-title text-center">{this.state.data.title}</h5>
                       <div className="card-body">{this.state.data.description}
                       </div>
                   </div>;

        }
        return <div className={this.state.data.state.name + " "}>
            <div className="card-header text-center">
                <div className="btn-group-justified" role="group">
                    <button className="btn btn-outline-light  " onClick={this.onClickDelete}>Delete</button>
                    <button className="btn btn-outline-light  " onClick={this.onClickUpdate}>Update</button>
                    <button className="btn btn-outline-light  " onClick={this.onClickChangeStatus}>=></button>
                </div>
            </div>
            <h5 className="card-title text-center">{this.state.data.title}</h5>
            <div className="card-body">{this.state.data.description}
            </div>
        </div>;
    }
}


class TaskDashboard extends React.Component {

    constructor(props) {
        super(props);
        this.state = { card: [] };

        this.onRemoveCard = this.onRemoveCard.bind(this);
        this.onUpdateCard = this.onUpdateCard.bind(this);
        this.onChangeStatus = this.onChangeStatus.bind(this);
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

    onUpdateCard(id, uTitle, uDescription, context) {
        var data = JSON.stringify({ "id": id, "title": uTitle, "description": uDescription });
        var xhr = new XMLHttpRequest();

        xhr.open("post", this.props.apiUrl + "/updatecard", true);
        xhr.setRequestHeader("Content-type", "application/json");
        xhr.onload = function () {
            if (xhr.status == 200) {
                var data = JSON.parse(xhr.responseText);
                context.setState({ data: data });
            }
        }.bind(this);
        xhr.send(data);
    }

    onChangeStatus(card, context) {
        //var data = JSON.stringify({ "id": card.id});
        var xhr = new XMLHttpRequest();

        xhr.open("get", this.props.apiUrl + "/changestatuscard/" + card.id, true);
        xhr.setRequestHeader("Content-type", "application/json");
        xhr.onload = function () {
            if (xhr.status == 200) {
                var data = JSON.parse(xhr.responseText);
                context.setState({ data: data });
                this.loadData();
                this.render();
            }
        }.bind(this);
        //xhr.send(data);
        xhr.send();
    }
    //<CardForm onCardSubmit={this.onAddCard} />
    render() {
        var remove = this.onRemoveCard;
        var update = this.onUpdateCard;
        var changeStatus = this.onChangeStatus;
        return <div className="dashboard">
            <h1 className="display-1 text-center text-capitalize">Dashboard</h1>
            <a class="badge badge-primary" href={this.props.apiUrl + "/create"}><h2>Add task</h2></a>

            <ul className="row display-4">
                <li className="col-md-4 text-center ">To Do</li>
                <li className="col-md-4 text-center">In Progress</li>
                <li className="col-md-4 text-center">Done</li>
            </ul>
            <div className="row">
                <div className="col-3 offset-1">
                    {
                        this.state.card.map(function (card) {
                            if (card.state.name == "ToDo")
                                return <div className="card text-white bg-danger"><TaskCard className="" key={card.id} card={card} onRemove={remove} onUpdate={update} onChangeStatus={changeStatus} />
                                </div>
                        })
                    }
                </div>
                <div className="col-3 offset-1">
                    {
                        this.state.card.map(function (card) {
                            if (card.state.name == "InProgress")
                                return <div className="card text-white bg-warning "><TaskCard className="" key={card.id
                                } card={card} onRemove={remove} onUpdate={update} onChangeStatus={changeStatus} />
                                </div>
                        })
                    }
                </div>
                <div className="col-3 offset-1">
                    {
                        this.state.card.map(function (card) {
                            if (card.state.name == "Done")
                                return <div className="card text-white bg-success "><TaskCard className="" key={card.id
                                } card={card} onRemove={remove} onUpdate={update} onChangeStatus={changeStatus} />
                                </div>
                        })
                    }
                </div>
            </div>
            </div>;
    }
}

ReactDOM.render(
    <TaskDashboard apiUrl="/dashboard" />,
    document.getElementById("content")
);