$(document).ready(function () {
    $("#pwd").keyup(function (event) {
        if (event.which === 13) {
            $("#btnConfirmar").click();
        }
    });

    $("#submit").click(function () {
        alert('clicked!');
    })
});
const autorizacion = () => {
    let pass = document.getElementById("pwd").value;
    if (pass != '' ) {
        if (pass == "4dm1n15tr4r") {
            $('#ModalAdministrar').modal('show'); //Abrir modal administrar
            $('#ModalAutorizacion').modal('toggle'); //Cerra modal autorización
            $('#pwd').val('');
        } 
        else if (pass == "4ju5t4r") {
            $('#ModalAjustar').modal('show'); //Abrir modal ajustar
            $('#ModalAutorizacion').modal('toggle'); //Cerra modal autorización
            $('#pwd').val('');
        } else {
            alert('¡La contraseña es incorrecta!');
        }
    } else {
        alert('Debe ingresar una contraseña para continuar...');
    }
    
}