class AjaxClient {
    get(url, callback) {
        this._ajax('GET', url, null, callback);
    }

    post(url, data, callback) {
        this._ajax('POST', url, data, callback);
    }

    delete(url, data, callback) {
        this._ajax('POST', url, data, callback);
    }

    _ajax(type, url, data, callback) {
        $.ajax({
            type,
            url,
            data,
            success: callback || (() => {}),
            error: (x, t, e) => this._onError(x, t, e)
        });
    }

    _onError(jqXHR, textStatus, errorThrown) {
        console.error("Во время обработки запроса возникла непредвиденная ситуация");
        console.log(jqXHR, textStatus, errorThrown);
    }
}

class Manager {
    constructor(containerSelector) {
        this._ajaxClient = new AjaxClient();

        this.container = $(containerSelector);
        this.container.on('click', '.delete', e => this._onDeleteClick(e));

        this.load();
    }

    _load(url, callback) {
        this._ajaxClient.get(url, callback);
        setTimeout(() => this.load(), 2500);//polling, не самое эффективное, но для небольшого проекта оптимальное решение
    }

    _post(url, data) {
        this._ajaxClient.post(url, data);
    }

    _delete(url, id) {
        this._ajaxClient.delete(`${url}/${id}`, null, () => this._deleteFromDom(id));
    }

    _getTableBody() {
        return this.container.find('.table > tbody');
    }

    _render(data) {
        this._getTableBody().empty();

        for (let d of data)
            this._appendToDom(d);
    }

    _appendToDom(d) {
        let table = this._getTableBody();
        let row = this.createRow(d);

        this._appendDeleteButton(row, d);
        table.append(row);
    }

    _appendDeleteButton(row, d) {
        if (this.deletable(d))
            row.append(`<td>
                <button class="btn btn-danger delete" data-id="${d.Id}">
                    <i class="glyphicon glyphicon-trash"></i>
                </button>
            </td>`);
        else
            row.append(`<td></td>`);
    }

    _deleteFromDom(id) {
        var table = this._getTableBody();
        table.find(`[data-id="${id}"]`).remove();
    }

    _onDeleteClick(e) {
        let id = $(e.currentTarget).attr('data-id');
        this.delete(id);
    }
}

class TaskManager extends Manager {
    load() {
        this._load('/tasks', tasks => this._render(tasks));
    }

    post(task) {
        this._post('/tasks/create', task);
    }

    delete(id) {
        this._delete('/tasks/delete', id);
    }

    deletable(task) {
        return task.Status !== 'Done';
    }

    createRow(task) {
        return $(`<tr data-id="${task.Id}">
            <th scope="row">${task.Description}</th>
            <td>${task.TimeSpent}</td>
            <td>${task.Handler}</td>
            <td>${task.Status}</td>
        </tr>`);
    }

    create(description) {
        this.post({ description });
    }
}

class EmployeeManager extends Manager {
    load() {
        this._load('/employees', employees => this._render(employees));
    }

    post(employee) {
        this._post('/employees/create', employee);
    }

    delete(id) {
        this._delete('/employees/delete', id);
    }

    deletable(employee) {
        return employee.Position !== 'Director';
    }

    createRow(employee) {
        return $(`<tr data-id="${employee.Id}">
            <th scope="row">${employee.Name}</th>
            <td>${employee.Position}</td>
            <td>${employee.IsBusy ? 'Занят' : 'Свободен'}</td>
        </tr>`);
    }

    create(name, position) {
        this.post({ name, position });
    }
}

class Configuration {
    constructor() {
        this._ajaxClient = new AjaxClient();
    }

    load(callback) {
        this._ajaxClient.get('/configuration', callback);
    }

    set(configuration) {
        this._ajaxClient.post('/configuration/update', configuration);
    }
}

$(() => {
    let taskManager = new TaskManager('.tasks-container');
    let employeeManager = new EmployeeManager('.employees-container');
    let configuration = new Configuration();

    $('.add-task').click(() => {
        let textarea = $('#task-description');
        let description = textarea.val();

        if (description !== '')
            taskManager.create(description);

        textarea.val('');
    });

    $('.add-employee').click(() => {
        let input = $('#employee-name');
        let select = $('#employee-position');
        let name = input.val();
        let position = select.val();

        if (name !== '')
            employeeManager.create(name, position);

        input.val('');
    });

    $('.update-configuration').click(() => {
        let rangeMin = $('#rangeMin').val();
        let rangeMax = $('#rangeMax').val();
        let tm = $('#tm').val();
        let td = $('#td').val();

        if (rangeMax < rangeMin)
            return alert("Верхняя граница должна быть больше нижней");

        configuration.set({ rangeMin, rangeMax, tm, td });
    });

    configuration.load(config => {
        $('#rangeMin').val(config.RangeMin);
        $('#rangeMax').val(config.RangeMax);
        $('#tm').val(config.Tm);
        $('#td').val(config.Td);
    });
});