var urlApi                   = 'https://localhost:6001' //Trocar para Pegar do COnfig !!! // IIS 1387/32780
var tabelaEventos            = null;
var eventoSelecionado        = {};
var modalAction              = null;
var listaSalas               = null;

$(document).ready(function ()
{
    $(":input[data-inputmask-mask]").inputmask();
    $(":input[data-inputmask-alias]").inputmask();
    $(":input[data-inputmask-regex]").inputmask("Regex");

    var listaEventos = ControllerRequest('GET', urlApi + '/api/eventos', {}, false);

    var listColumns = [
        { "className": 'details-view-control', "orderable": false, "data": null, "defaultContent": '' },
        {"className": 'details-control-edit',"orderable": false,"data": null,"defaultContent": '' },
        {"className": 'details-control-delete',"orderable": false,"data": null, "defaultContent": ''},

        { "data": "eventoId" },    { "data": "salaId" },    { "data": "salaNome" },
        { "data": "dataInicial" }, { "data": "dataFinal" }, { "data": "responsavel" }];


    listaEventos.forEach(function (elem, i) {
        elem.dataInicial = FormatDate(elem.dataInicial);
        elem.dataFinal = FormatDate(elem.dataFinal);
    });

    tabelaEventos = ConstruirTabela('#tabelaEventos', listaEventos, listColumns, true, [6, 'asc']);


    $('#tabelaEventos tbody').on('click', 'tr td.details-view-control', function () {
        modalAction = 'view';

        $('#eventosModal').modal('show');
        eventoSelecionado = tabelaEventos.row(this).data();
    });

    $('#listaEventosTbody').on('click', 'td.details-control-edit', function ()
    {
        modalAction = 'edit';
        $('#eventosModal').modal('show');
        eventoSelecionado = tabelaEventos.row(this).data();
    });

    $('#listaEventosTbody').on('click', 'td.details-control-delete', function ()
    {
        modalAction = 'delete';
        var rowData = tabelaEventos.row(this).data();

        var result = confirm("Deseja excluir esse evento?");
        if (result)
        {
            ControllerRequestCallBack('DELETE', urlApi + '/api/eventos/' + rowData.eventoId, {}, true, ObterLista);
        }
    });

    $('#eventosModal').on('shown.bs.modal', function () {

        if (modalAction == 'view') {
            $('#hInfoModalPropostaModal').html('Visualização do Evento');
        }
        else if (modalAction == 'edit') {
            $('#hInfoModalPropostaModal').html('Edição do Evento');
        }
        else if (modalAction == 'create')
        {
            $('#hInfoModalPropostaModal').html('Criação do Evento');
        }

        GetSalas();

        preencherDropDown($('#ddlSala'), listaSalas, true);

        if (eventoSelecionado != null) {
            $('#ddlSala').val(eventoSelecionado.salaId);


            $('#txtDataInicial').val(eventoSelecionado.dataInicial);
            $('#txtDataFinal').val(eventoSelecionado.dataFinal);
            $('#txtResponsavel').val(eventoSelecionado.responsavel);
        }

        ReadOnly();
    });

    $('#eventosModal').on('hidden.bs.modal', function ()
    {
        //Limpar dados do modal
        eventoSelecionado = null;
    });

});

function ReadOnly()
{
    if (modalAction == 'view')
    {
        $('#ddlSala').prop('disabled', true);
        $('#txtDataInicial').prop('disabled', true);
        $('#txtDataFinal').prop('disabled', true);
        $('#txtResponsavel').prop('disabled', true);

        $('#btnSalvar').css('display', 'none');
    }
    else if (modalAction == 'edit')
    {
        $('#ddlSala').prop('disabled', true);
        $('#txtDataInicial').prop('disabled', false);
        $('#txtDataFinal').prop('disabled', false);
        $('#txtResponsavel').prop('disabled', false);

        $('#btnSalvar').css('display', 'block');
    }
    else if (modalAction == 'create')
    {
        $('#ddlSala').prop('disabled', false);
        $('#txtDataInicial').prop('disabled', false);
        $('#txtDataFinal').prop('disabled', false);
        $('#txtResponsavel').prop('disabled', false);

        $('#btnSalvar').css('display', 'block');
    }
}

function GetSalas()
{
    if (listaSalas == null)
    {
        listaSalas = ControllerRequest('GET', urlApi + '/api/salas/combo', {}, false);
    }
}

function ConstruirTabela(id, lista, columnsList, showFilter, orderList, drawCallback = undefined, _lengthMenuValue = undefined, _lengthMenuText = undefined, _pageLength = 5) {
    var lengthMenuValue = (_lengthMenuValue == undefined ? [5, 10, 20, 30, 40, -1] : _lengthMenuValue);
    var lengthMenuText = (_lengthMenuText == undefined ? ['5', '10', '20', '30', '40', 'Exibir todos'] : _lengthMenuText);

    var table = $(id).DataTable({
        language: {
            "url": "../../json-language/Portuguese-Brasil.json"
        },
        bFilter: showFilter, //Searchbox
        lengthChange: true,
        pagingType: "first_last_numbers",
        data: lista,
        columns: columnsList,
        pageLength: _pageLength,
        order: [orderList],
        dom: 'Bfrtip',
        lengthMenu: [lengthMenuValue, lengthMenuText],
        buttons: ['pageLength', 'copy', 'csv', 'excel', 'pdf', 'print', 'colvis'],

        "drawCallback": function (settings) {
            if (drawCallback != undefined && drawCallback != null) {
                drawCallback();
            }
        }
    });
    return table;
}

function ObterLista(data)
{
    var listaEventos = ControllerRequest('GET', urlApi + '/api/eventos', {}, false);

    listaEventos.forEach(function (elem, i) {
        elem.dataInicial = FormatDate(elem.dataInicial);
        elem.dataFinal = FormatDate(elem.dataFinal);
    });

    if (data.sucesso) {
        tabelaEventos.clear().draw();
        tabelaEventos.rows.add(listaEventos); // Add new data
        tabelaEventos.columns.adjust().draw(); // Redraw the DataTable

        switch (modalAction) {
            case 'edit':
                alert('Atualização efetuada com sucesso!');
                $('#eventosModal').modal('toggle');
                break;
            case 'create':
                alert('Criação efetuada com sucesso!');
                $('#eventosModal').modal('toggle');
                break;
            case 'delete':
                alert('Exclusão efetuada com sucesso!');
                break;
                
            default:
                console.log('Comando inválido ${modalAction}.');
        }
    }

    modalAction = null;
}


function SalvarDados()
{
    if (validaCampos()) {
        var isPost = (modalAction === 'create' ? true : false);

        var dados = JSON.stringify(
            {
                EventoId: (isPost ? null : eventoSelecionado.eventoId),
                SalaId: parseInt((isPost ? $('#ddlSala').val() : eventoSelecionado.salaId)),
                DataInicial: $('#txtDataInicial').val().split("/").reverse().join("-"),
                DataFinal: $('#txtDataFinal').val().split("/").reverse().join("-"),
                Responsavel: $('#txtResponsavel').val(),
            });

        ControllerRequestCallBack((isPost ? 'POST' : 'PUT'), urlApi + '/api/eventos', dados, true, ObterLista);
    }
}

function CriarEvento()
{
    LimparCampos();

    modalAction = 'create';
    $('#eventosModal').modal('show');
}

function LimparCampos()
{
    $('#ddlSala').val('');
    $('#txtDataInicial').val('');
    $('#txtDataFinal').val('');
    $('#txtResponsavel').val('');
}

function validaCampos()
{
    var campos = ""

    if ($('#ddlSala').val() == "")
        campos += "\n- Sala";
    if ($('#txtDataInicial').val() == "")
        campos += "\n- Data Inicial";
    if ($('#txtDataFinal').val() == "")
        campos += "\n- Data Final";

    campos += verificaDatas($('#txtDataInicial').val(),$('#txtDataFinal').val());

    if ($('#txtResponsavel').val() == "")
        campos += "\n - Responsável";

    if (campos != "")
    {
        alert('O(s) campo(s) abaixo devem ser preenchidos:' + campos);
        return false;
    }
    else
    {
        return true;
    }
}

function verificaDatas(dtInicial, dtFinal) {

    var dtini = dtInicial;
    var dtfim = dtFinal;

    if ((dtini == '') && (dtfim == ''))
    {
        return "";
    }

    datInicio = new Date(dtini.substring(6, 10),
        dtini.substring(3, 5),
        dtini.substring(0, 2));
    datInicio.setMonth(datInicio.getMonth() - 1);


    datFim = new Date(dtfim.substring(6, 10),
        dtfim.substring(3, 5),
        dtfim.substring(0, 2));

    datFim.setMonth(datFim.getMonth() - 1);

    if (datInicio <= datFim)
    {
        return "";    
    }
    else
    {
        return "\n - Data Inicial é maior que Data Final";
    }

}