$(function() {

    $('#AcceptUserAgreement').click(onToggleRegisterUserDisablesClick)

    function onToggleRegisterUserDisablesClick() {
        $('.register-user-panel button').toggleClass("disabled");
    }

    $('.register-user-panel button').click(
        onRegisterUserClick);

    function onRegisterUserClick () {

        var url = '/Account/RegisterUserAsync';
        var antiforgerytoken = $('[name="__RequestVerificationToken"]').val();
        var name = $('.register-user-panel .first-name').val();
        var email = $('.register-user-panel .email').val();
        var pwd = $('.register-user-panel .password').val();

        $.post (url,
            {
                __RequestVerificationToken: antiforgerytoken,
                name: name,
                email: email,
                password: pwd,
                AcceptUserAgreement: true
            }, 
            function (data) {

                var parsed = $.parseHTML(data);

                var hasErrors = $(parsed).find('[data-valmsg-summary]')
                    .text().replace(/\n|\r/g, "").length > 0;

                if(hasErrors) {
                    $('.register-user-panel').html(data);
                    $('#AcceptUserAgreement').click(onToggleRegisterUserDisablesClick);
                    $('.register-user-panel button').click(onRegisterUserClick);
                    $('.register-user-panel button').remove("disabled");
                }
                else {
                    $('#AcceptUserAgreement').click(onToggleRegisterUserDisablesClick);
                    $('.register-user-panel button').click(onRegisterUserClick);
                    location.href = '/Home/Index';
                }

            }).fail(function(xhr, status, error) {
                alert("Post Unsuccessful");
            });
    }
});