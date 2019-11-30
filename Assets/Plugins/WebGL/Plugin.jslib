var plugin = {

    ShowLoginButton: function()
    {
        if (window.document.getElementsByClassName('g-signin2').length > 0)
            window.document.getElementsByClassName('g-signin2')[0].style.display = 'block';
    },

    HideLoginButton: function() {
        if (window.document.getElementsByClassName('g-signin2').length > 0)
            window.document.getElementsByClassName('g-signin2')[0].style.display = 'none';
    }

};

mergeInto(LibraryManager.library, plugin);