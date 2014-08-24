
(function() {
    var tagnames = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        prefetch: {
            url: '../Home/GetTagStrings',
            filter: function(list) {
                return $.map(list, function(tagname) {
                    return { name: tagname };
                });
            }
        }
    });

    tagnames.initialize();

    $('#taginput').tagsinput({
        typeaheadjs: {
            name: 'tagnames',
            displayKey: 'name',
            valueKey: 'name',
            source: tagnames.ttAdapter()
        }
    });
})();