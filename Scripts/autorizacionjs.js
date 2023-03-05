$(document).ready(function () {
    $("#pwd").keyup(function (event) {
        if (event.which === 13) {
            $("#btnConfirmar").click();
        }
    });
    $("#tTurno").keyup(function (event) {
        if (event.which === 13) {
            $("#btnConfirmar").click();
        }
    });
    $("#submit").click(function () {
        alert('clicked!');
    })
    $('.decimales').on('input', function () {
        this.value = this.value.replace(/[^0-9,.]/g, '').replace(/,/g, '.');
    });
});
const autorizacion = () => {
    let pass = document.getElementById("pwd").value;
    let tipoTurno = document.getElementById("tTurno").value;
    if (pass != '') {
        if (tipoTurno != '') {
            if (pass == "4dm1n15tr4r" && tipoTurno == "a") {
                $('#ModalAdministrarHombres').modal('show'); //Abrir modal administrar
                $('#ModalAutorizacion').modal('toggle'); //Cerra modal autorización
                $('#pwd').val('');
                $('#tTurno').val('');
            }
            else if (pass == "4dm1n15tr4r" && tipoTurno == "rm") {

                $('#ModalAdministrarMujeres').modal('show'); //Abrir modal administrar
                $('#ModalAutorizacion').modal('toggle'); //Cerra modal autorización
                $('#pwd').val('');
                $('#tTurno').val('');
            } else if (pass == "4dm1n15tr4r" && tipoTurno == "d") {
                $('#ModalAdministrarProductos').modal('show'); //Abrir modal administrar
                $('#ModalAutorizacion').modal('toggle'); //Cerra modal autorización
                $('#pwd').val('');
                $('#tTurno').val('');
            }
            else if (pass == "4ju5t4r" && tipoTurno == "a") {
                $('#ModalAjustarHombres').modal('show'); //Abrir modal ajustar
                $('#ModalAutorizacion').modal('toggle'); //Cerra modal autorización
                $('#pwd').val('');
                $('#tTurno').val('');
            }
            else if (pass == "4ju5t4r" && tipoTurno == "rm") {
                $('#ModalAjustarMujeres').modal('show'); //Abrir modal ajustar
                $('#ModalAutorizacion').modal('toggle'); //Cerra modal autorización
                $('#pwd').val('');
                $('#tTurno').val('');
            }
            else if (pass == "4dm1n15tr4r" && tipoTurno == "c") {
                $('#ModalAdministrarCumpleanios').modal('show'); //Abrir modal administrar
                $('#ModalAutorizacion').modal('toggle'); //Cerra modal autorización
                $('#pwd').val('');
                $('#tTurno').val('');
            }
            else {
                alert('¡La contraseña es incorrecta!');
                $('#pwd').val('');
                $('#tTurno').val('');
            }
        } else {
            alert('Debe ingresar el tipo de turno para continuar');
        }
       
    } else {
        alert('Debe ingresar una contraseña para continuar...');
    }
    
}