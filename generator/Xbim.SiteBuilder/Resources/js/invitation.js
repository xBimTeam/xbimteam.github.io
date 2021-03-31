(
    function () {
        var dismissed = localStorage.getItem('invitationDismissed') === 'true';
        if (dismissed) {
            var ago = Date.now() - parseInt(localStorage.getItem('invitationDismissedOn'));
            if (ago < 7 * 24 * 60 * 60 * 1000) {
                // dismissed less than a week ago, so don't bother
                return;
            }
        }

        setTimeout(function () {
            $('#invitation').collapse('show');
        }, 2000)
        $('#btn-invitation-dismiss').click(function () {
            $('#invitation').collapse('hide');
            localStorage.setItem('invitationDismissed', 'true');
            localStorage.setItem('invitationDismissedOn', Date.now());
        })
    }
)();