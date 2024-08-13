// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('.close-alert').click(function () {
    $('.alert').hide('hide');
});

function mostrarDetalhes(codLivro) {
    $.ajax({
        url: '/Livro/ListarDetalhes',
        data: { codLivro: codLivro },
        success: function (data) {
            $('#modal-content').html(data);
            $('#detalhesModal').modal('show');
        }
    });
}




