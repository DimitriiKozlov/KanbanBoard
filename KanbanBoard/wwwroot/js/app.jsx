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
        return <div>
            <p><b>{this.state.data.title}</b></p>
            <p>{this.state.data.description}</p>
            <p><button onClick={this.onClick}>Delete</button></p>
        </div>;
    }
}

//class PhoneForm extends React.Component {

//    constructor(props) {
//        super(props);
//        this.state = { name: "", price: 0 };

//        this.onSubmit = this.onSubmit.bind(this);
//        this.onNameChange = this.onNameChange.bind(this);
//        this.onPriceChange = this.onPriceChange.bind(this);
//    }
//    onNameChange(e) {
//        this.setState({ name: e.target.value });
//    }
//    onPriceChange(e) {
//        this.setState({ price: e.target.value });
//    }
//    onSubmit(e) {
//        e.preventDefault();
//        var phoneName = this.state.name.trim();
//        var phonePrice = this.state.price;
//        if (!phoneName || phonePrice <= 0) {
//            return;
//        }
//        this.props.onPhoneSubmit({ name: phoneName, price: phonePrice });
//        this.setState({ name: "", price: 0 });
//    }
//    render() {
//        return (
//            <form onSubmit={this.onSubmit}>
//                <p>
//                    <input type="text"
//                        placeholder="Модель телефона"
//                        value={this.state.name}
//                        onChange={this.onNameChange} />
//                </p>
//                <p>
//                    <input type="number"
//                        placeholder="Цена"
//                        value={this.state.price}
//                        onChange={this.onPriceChange} />
//                </p>
//                <input type="submit" value="Сохранить" />
//            </form>
//        );
//    }
//}


class TaskDashboard extends React.Component {

    constructor(props) {
        super(props);
        this.state = { card: [] };

        //this.onAddPhone = this.onAddPhone.bind(this);
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
    //onAddPhone(phone) {
    //    if (phone) {

    //        var data = JSON.stringify({ "name": phone.name, "price": phone.price });
    //        var xhr = new XMLHttpRequest();

    //        xhr.open("post", this.props.apiUrl, true);
    //        xhr.setRequestHeader("Content-type", "application/json");
    //        xhr.onload = function () {
    //            if (xhr.status == 200) {
    //                this.loadData();
    //            }
    //        }.bind(this);
    //        xhr.send(data);
    //    }
    //}
    // удаление объекта
    onRemoveCard(card) {

        if (card) {
            var url = this.props.apiUrl + card.id;

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
    render() {
////<PhoneForm onPhoneSubmit={this.onAddPhone} />
        var remove = this.onRemoveCard;
        return <div>
            <h2>Dashboard</h2>
            <div>
                {
                    this.state.card.map(function (card) {

                        return <TaskCard key={card.id} card={card} onRemove={remove} />
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