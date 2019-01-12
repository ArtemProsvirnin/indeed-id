class Manager {
    constructor(containerSelector, addButtonSelector) {
        this.container = $(containerSelector);
        this.addButton = $(addButtonSelector);

        this.container.on('click', '.delete', e => this._onDeleteClick);
        this.addButton.click(e => this._onAddClick(e));
    }

    _load(url, callback) {
        this._ajax('GET', url, null, callback);
    }

    _post(url, data, callback) {
        this._ajax('POST', url, data, callback);
    }

    _delete(url, data, callback) {
        this._ajax('DELETE', url, data, callback);
    }

    _ajax(type, url, data, success) {
        $.ajax({
            type,
            url,
            data,
            success,
            error: (x, t, e) => this.onError(x, t, e)
        });
    }

    _onError(jqXHR, textStatus, errorThrown) {
        console.error("Во время обработки запроса возникла непредвиденная ситуация");
        console.log(jqXHR, textStatus, errorThrown);
    }

    _getTable() {
        return container.children('.table');
    }

    _onDeleteClick(e) {
        console.log(e);
        debugger;
    }
}

class TaskManager extends Manager {
    load() {
        this._load('/tasks', tasks => this.render(tasks));
        setInterval(() => this.load(), 2500);//polling, не самое эффективное, но для небольшого проекта оптимальное решение
    }

    post(task) {
        this._post('/tasks/add', task, t => this.appendToDom(t));
    }

    delete(task) {
        this._delete('/tasks/delete', task, () => this.deleteFromDom(task));
    }

    render(tasks) {
        var table = getTable();
        console.log(tasks);
    }

    appendToDom(task) {
        var table = this._getTable();
        console.log(task);
    }

    deleteFromDom(task) {
        var table = this._getTable();
        console.log(task);
    }

    _onAddClick(e) {
        console.log(e);
    }
}

class EmployeeManager extends Manager {
    load() {
        this._load('/employees', employees => this.render(employees));
    }

    post(employee) {
        this._post('/employees/add', employee, e => this.appendToDom(e));
    }

    delete(employee) {
        this._delete('/employees/delete', employee, () => this.deleteFromDom(employee));
    }

    render(employees) {
        var table = getTable();
        console.log(employees);
    }

    appendToDom(employee) {
        var table = this._getTable();
        console.log(employee);
    }

    deleteFromDom(employee) {
        var table = this._getTable();
        console.log(employee);
    }

    _onAddClick(e) {
        console.log(e);
    }
}

$(() => {
    var taskManager = new TaskManager('.tasks-container', '.add-task');
    var employeeManager = new EmployeeManager('.employees-container', '.add-employee');
});