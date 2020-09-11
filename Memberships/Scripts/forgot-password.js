$(function () {

    $('#pwdLink').hover(onCloseLogin);
    $('button#resetPwd').click(onResetPassword);

    function onCloseLogin() {
        $('div[data-login-user-area]').removeClass('open');
    }

    function onResetPassword() {
        var email = $('input.reset-email').val();
        var token = $('[name="__RequestVerificationToken"]').val();

        var url = "/Account/ForgotPasswordConfirmation";

        $.post("/Account/ForgotPassword", {
            __RequestVerificationToken: token, email: email
        }, function (data) {
            location.href = url;
        }).fail(function (xhr, status, error) { location.href = url; });
    }
})