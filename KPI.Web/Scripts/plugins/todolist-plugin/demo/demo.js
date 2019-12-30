$(function () {
    //Custom datepicker
    $('#todo-lists-demo-datepicker').lobiList({
        // Available style for lists
        'listStyles': ['lobilist-default', 'lobilist-danger', 'lobilist-success', 'lobilist-warning', 'lobilist-info', 'lobilist-primary'],
        // Default options for all lists
        listsOptions: {
            id: false,
            title: '',
            items: []
        },
        // Default options for all todo items
        itemOptions: {
            id: false,
            title: '',
            description: '',
            dueDate: '',
            done: false
        },

        lists: [],
        // Urls to communicate to backend for todos
        actions: {
            'load': '/ChartPeriod/LoadToDo',
            'update': '',
            'insert': '',
            'delete': ''
        },
        // Whether to show checkboxes or not
        useCheckboxes: true,
        // Show/hide todo remove button
        enableTodoRemove: true,
        // Show/hide todo edit button
        enableTodoEdit: true,
        // Whether to make lists and todos sortable
        sortable: true,
        // Default action buttons for all lists
        controls: ['edit', 'add', 'remove', 'styleChange'],
        //List style
        defaultStyle: 'lobilist-danger',
        // Whether to show lists on single line or not
        onSingleLine: true,


        init: function () {
            //console.log("Da tao TODO");
            //console.log(arguments);
            //console.log(this.$options);
        },
        beforeDestroy: function () {
            //console.log("Da tao TODO");
        },
        afterDestroy: function () {

        },
        beforeListAdd: function () {

        },
        afterListAdd: function (lobilist, list) {
            //console.log("Console lobilist");
            //console.log(lobilist);
            //console.log("Console list");

        },
        beforeListRemove: function () {

        },
        afterListRemove: function () {

        },
        beforeItemAdd: function () {
            var KPILevelCodeAndPeriod = getUrlParameter('kpilevelcode') + getUrlParameter('period');
            var me = this;
            var item = me.$options;
            console.log(me);
            var obj = {
                ID: arguments[1].id,
                UserID: $('#user').data('userid'),
                DataID: $('.dataid').text(),
                CommentID: Number($('.commentid').text()),
                Title: arguments[1].title,
                KPILevelCodeAndPeriod: KPILevelCodeAndPeriod,
                Description: arguments[1].description,
                Content: " ",
                ApprovedBy: 0,
                CreateTime: new Date(),
                Deadline: arguments[1].dueDate,
                SubmitDate: new Date(),
                Status: false,
                ApprovedStatus: false,
                ActionPlanCategoryID: item.id

            };
            $.post('Add', { item: obj }, function (data) {
                console.log(data);
            });
        },
        afterItemAdd: function (lobilist, list) {
            //console.log("Click add");
            //console.log(arguments[1]);
            //$.post('Add', { item: arguments[1] }, function (data) {
            //    console.log(data);
            //});
        },
        beforeItemUpdate: function () {
            var KPILevelCodeAndPeriod = getUrlParameter('kpilevelcode') + getUrlParameter('period');
            var me = this;
            var item = me.$options;
            console.log(me);
            var obj = {
                ID: arguments[1].id,
                UserID: $('#user').data('userid'),
                DataID: $('.dataid').text(),
                CommentID: Number($('.commentid').text()),
                Title: arguments[1].title,
                KPILevelCodeAndPeriod: KPILevelCodeAndPeriod,
                Description: arguments[1].description,
                Content: " ",
                ApprovedBy: 0,
                CreateTime: new Date(),
                Deadline: arguments[1].dueDate,
                SubmitDate: new Date(),
                Status: false,
                ApprovedStatus: false,
                ActionPlanCategoryID: item.id

            };
            $.post('Update', { item: obj }, function (data) {
                console.log(data);
            });
        },
        afterItemUpdate: function () {

        },
        beforeItemDelete: function () {

            $.post('Delete', { id: arguments[1].id }, function (data) {
                console.log(data);
            });
        },
        afterItemDelete: function () {

        },
        beforeListDrop: function () {

        },
        afterListReorder: function () {

        },
        beforeItemDrop: function () {

        },
        afterItemReorder: function () {

        },
        afterMarkAsDone: function () {

        },
        afterMarkAsUndone: function () {

        },
        styleChange: function (list, oldStyle, newStyle) {

        },
        titleChange: function (list, oldTitle, newTitle) {
            //console.log(list);
            //console.log(oldTitle);
            //console.log(newTitle);
        }
        //   actions: {
        //    'load': '/ChartPeriod/LoadToDo',
        //       'update': '/ChartPeriod/Update',
        //    'insert': '',
        //    'delete': ''
        //}
    });

    //get the LobiList instance
    var $instance = $('#todo-lists-demo-datepicker').data('lobiList');
    console.log($instance);
    //call the methods       
    var a = $instance.$lists.length;
    if (a <= 0) {
        $instance.addList({
            title: 'To Do list',
            defaultStyle: 'lobilist-info',
            useCheckboxes: true,
            item: []
        });

    }

});
function getUrlParameter (sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
}