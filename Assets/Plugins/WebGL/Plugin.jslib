var plugin = {

    ShowLoginButton: function()
    {
        console.log(window);
        window.document.getElementsByClassName('g-signin2')[0].style.display = 'auto';
    },

    HideLoginButton: function() {
        window.document.getElementsByClassName('g-signin2')[0].style.display = 'none';
    }

};

mergeInto(LibraryManager.library, plugin);