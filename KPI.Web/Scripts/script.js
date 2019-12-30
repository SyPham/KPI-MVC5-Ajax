$(document).ready(function () {


    $('.tooltip-ui').tooltip();

   function getDateOfWeekInYear(dt)
    {
        var tdt = new Date(dt.valueOf());
        var dayn = (dt.getDay() + 6) % 7;
        tdt.setDate(tdt.getDate() - dayn + 3);
        var firstThursday = tdt.valueOf();
        tdt.setMonth(0, 1);
        if (tdt.getDay() !== 4) {
            tdt.setMonth(0, 1 + ((4 - tdt.getDay()) + 7) % 7);
        }
        return 1 + Math.ceil((firstThursday - tdt) / 604800000);
    }

    $('[data-toggle="tooltip"]').tooltip();
    //$('.monthly').datetimepicker({
    //    format: 'MM/DD/YYYY'
    //});
    //$('.quaterly').datetimepicker({
    //    format: 'MM/DD/YYYY'
    //});
    //$('.yearly').datetimepicker({
    //    format: 'MM/DD/YYYY'
    //});

    $('#ChangePassword .username').val($('#user').text().trim());
    $('#btnChangePassword').off('click').on('click', function () {
        if ($('#ChangePassword .username').val().trim() === "") {
            Swal.fire({
                title: 'Warning!',
                text: 'Please enter password!',
                type: 'warning'
            });
            return;
        }
            var  username= $('#ChangePassword .username').val();
            var  password= $('#ChangePassword .password').val();
        $.post("/AdminUser/ChangePassword", { username: username, password: password }, function (result) {
            if (result) {
                Swal.fire({
                    title: 'Success!',
                    text: 'Change password successfully!',
                    type: 'success'
                });
                $('#modal-group-change-password').modal('hide');
            }
            else {
                Swal.fire({
                    title: 'Error!',
                    text: 'Error!',
                    type: 'error'
                });
            }
        });


    });




});

