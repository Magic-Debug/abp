$(function () {
    var $container = $('#qa-new-post-container');
    var $editorContainer = $container.find('.new-post-editor');
    var $submitButton = $container.find('button[type=submit]');
    var $form = $container.find('form#new-post-form');
    var $titleLengthWarning = $('#title-length-warning');
    var maxTitleLength = parseInt($titleLengthWarning.data('max-length'));
    var $title = $('#Post_Title');
    var $url = $('#Post_Url');
    var $postFormSubmitButton = $('#PostFormSubmitButton');


    var newPostEditor = new toastui.Editor({
        el: $editorContainer[0],
        usageStatistics: false,
        initialEditType: 'markdown',
        previewStyle: 'tab',
        height: 'auto',
        plugins: [toastui.Editor.plugin.codeSyntaxHighlight],
        hooks: {
            addImageBlobHook: function (blob, callback, source) {
                var imageAltText = blob.name;

                //uploadImage(blob, function (webUrl) {
                //    callback(webUrl, imageAltText);
                //});
            },
        },
        events: {
            load: function () {
                $editorContainer.find('.loading-cover').remove();
                $submitButton.prop('disabled', false);
                $form.data('validator').settings.ignore = '.ignore';
                $editorContainer.find(':input').addClass('ignore');
            },
        },
    });

    $container.find('form#new-post-form').submit(function (e) {
        var $postTextInput = $form.find("input[name='Post.Content']");

        var postText = newPostEditor.getMarkdown();
        $postTextInput.val(postText);
        $submitButton.buttonBusy();
        $(this).off('submit').submit();
        return true;
    });

    var urlEdited = false;
    var reflectedChange = false;

    $title.on('change paste keyup', function () {
        if (urlEdited) {
            return;
        }

        var title = $title.val();

        if (title.length > maxTitleLength) {
            $titleLengthWarning.show();
        } else {
            $titleLengthWarning.hide();
        }

        title = title.replace(' &', ' ');
        title = title.replace('& ', ' ');
        title = title.replace('&', '');
        title = title.replace(' ', '-');
        title = title.replace('/', '-');
        title = title.replace(new RegExp(' ', 'g'), '-');
        reflectedChange = true;
        $url.val(title);
        reflectedChange = false;
    });

    $url.change(function () {
        if (!reflectedChange) {
            urlEdited = true;
        }
    });
});
