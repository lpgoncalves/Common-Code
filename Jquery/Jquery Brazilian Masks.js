//Works with jquery 1.0 >

// MASK 
$(function () {
    {
        $(".cnpj").mask("99.999.999/9999-99");
        $(".cpf").mask("999.999.999-99");
        $(".datetime").mask("99/99/9999");
        $(".shortdate").mask("99/99");
        $(".cep").mask("99999-999");

        $(".phone").mask(function (phone, e, currentField, options) {
            return phone.match(/^\((11|12|13|14|15|16|17|18|19|21|22|24|27|28|31|32|33|34|35|37|38|41|42|43|44|45|46|47|48|49|51|53|54|55|61|62|63|64|65|66|67|68|69|71|73|74|75|77|79|81|82|83|84|85|86|87|88|89|91|92|93|94|95|96|97|98|99)\)\s9(4[0-9]|5[0-9]|6[0-9]|7[01234569]|8[0-9]|9[0-9])[0-9]{1}/g) ? '(00) 00000-0000' : '(00) 0000-0000';
        },

                {
                    onKeyPress: function (phone, e, currentField, options) {
                        var match = phone.match(/^\((11|12|13|14|15|16|17|18|19|21|22|24|27|28|31|32|33|34|35|37|38|41|42|43|44|45|46|47|48|49|51|53|54|55|61|62|63|64|65|66|67|68|69|71|73|74|75|77|79|81|82|83|84|85|86|87|88|89|91|92|93|94|95|96|97|98|99)\)\s9(4[0-9]|5[0-9]|6[0-9]|7[01234569]|8[0-9]|9[0-9])[0-9]{1}/g);

                        if (match) {
                            $(currentField).mask("(00) 00000-0000", options);
                        }
                        else {
                            $(currentField).mask("(00) 0000-0000", options);
                        }
                    }
                }
            );

        $(".percentage").mask("999");
        $(".numeric").mask("999999");
        $(".numberCard").mask("9999.9999.9999.9999");
        $(".securityCard").mask("999");

        $(".cpfcnpj").mask("999.999.999-999",
            {
                "onKeyPress": function (text, event, field, options) {
                    var match = text.length <= 14;

                    match ? $(field).mask("000.000.000-000", options) : $(field).mask("00.000.000/0000-00", options);
                }
            }
        );

    }
});

//MÁSCARA PARA TELEFONE / CELULAR
$(function () {
    $(".phone").on('input propertychange', function () {

        var str = $(this).val();

        str = str.replace(/[^0-9]/gi, '');
        var match = str.match(/^(11|12|13|14|15|16|17|18|19|21|22|24|27|28|31|32|33|34|35|37|38|41|42|43|44|45|46|47|48|49|51|53|54|55|61|62|63|64|65|66|67|68|69|71|73|74|75|77|79|81|82|83|84|85|86|87|88|89|91|92|93|94|95|96|97|98|99)9(4[0-9]|5[0-9]|6[0-9]|7[01234569]|8[0-9]|9[0-9]){1}/g);

        if (match) {
            if (str.length == 1 || str.length == 2) {
                str = '(' + str;
            }

            else if (str.length >= 3 && str.length < 8) {
                str = '(' + str.substring(0, 2) + ') ' +
                    str.substring(2, str.length);
            }

            else if (str.length >= 8) {
                str = '(' + str.substring(0, 2) + ') ' +
                        str.substring(2, 7) + '-' +
                        str.substring(7, 11);
            }
        }

        else {
            if (str.length == 1 || str.length == 2) {
                str = '(' + str;
            }

            else if (str.length >= 3 && str.length < 7) {
                str = '(' + str.substring(0, 2) + ') ' +
                    str.substring(2, str.length);
            }

            else if (str.length >= 7) {
                str = '(' + str.substring(0, 2) + ') ' +
                        str.substring(2, 6) + '-' +
                        str.substring(6, 10);
            }
        }
        $(this).val(str);
        }
    );
});

//FORMATAR CPF / CNPJ NO TEXTBOX D BUSCA DO FLEXIGRID
function setSearchButton() {
    var e = document.getElementById("flexigridSearchOptions");
    var strOption = e.options[e.selectedIndex].value;

    if (strOption == "cpf") {

            var str = $('#flexigridSearchBox').val();

            str = str.replace(/[^0-9]/gi, '');

            if (str.length >= 4 && str.length < 7) {
                str = str.substring(0, 3) + '.' +
                    str.substring(3, str.length);
            }

            else if (str.length >= 7 && str.length < 10) {
                str = str.substring(0, 3) + '.' +
                        str.substring(3, 6) + '.' +
                        str.substring(6, str.length);
            }

            else if (str.length >= 10 && str.length < 12) {
                str = str.substring(0, 3) + '.' +
                        str.substring(3, 6) + '.' +
                        str.substring(6, 9) + '-' +
                        str.substring(9, 11);
            }

            else if (str.length == 12) {
                str = str.substring(0, 2) + '.' +
                        str.substring(2, 5) + '.' +
                        str.substring(5, 8) + '/' +
                        str.substring(8, 12);
            }

            else if (str.length > 12) {
                str = str.substring(0, 2) + '.' +
                        str.substring(2, 5) + '.' +
                        str.substring(5, 8) + '/' +
                        str.substring(8, 12) + '-' +
                        str.substring(12, 14);
            }

            $('#flexigridSearchBox').val(str);
        }
    }