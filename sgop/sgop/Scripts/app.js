
$(document).ready(function () {
    //------------------------------Log In-------------------------------------
    $('#idRecuperar').click(function (e) {
        e.preventDefault();
        let template = `<form  method="post" action="/Access/Recuperar" id="recuperarPass">
                        <div class="row">
                            <div class="col-12 text-center">
                                <h3 class="text-muted font-weight-bold">Ingrese los datos</h3>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12 my-3">
                                <div class="input-group input-group-sm w-75 mx-auto">
                                <div class="input-group-prepend">
                                    <span class="input-group-text fas fa-user-tie"></span>
                                </div>
                                <input type="text" class="form-control" required id="usuario" name="usuario" autofocus placeholder="Usuario">
                            </div>
                            </div>
                        </div>
                    <div class="row">
                        <div class="col-12 mb-3">
                            <div class="input-group input-group-sm w-75 mx-auto">
                               <div class="input-group-prepend">
                                   <span class="input-group-text fas fa-at"></span>
                               </div>
                               <input type="email" class="form-control" required id="email" name="email" placeholder="Correo">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <div class="w-75 mx-auto my-3">
                                <button type="submit" class="btn btn-success btn-block btn-sm">RECUPERAR<span class="fas fa-reply mx-1"></span></button>
                            </div>
                    </div>
                    </form>`;
        $('#recuperarPassword').html(template);

    });




    $(".admin").click(function (e) {
        e.preventDefault();
        var user = $('#user').val();
        var id = $(this).attr("id");

        if ((id != 1 && id != 6) && user != "") {
            url = "/Admin/BuscarId";
            const postData = { user: user };
            $.post(url, postData, function (data) {
                if (data.a == true) {

                    switch (id) {
                        case '2':
                            $('#userX').val($('#user').val());
                            $('#btn2Id').click();
                            break;
                        case '3':
                            $('#userN').val($('#user').val());
                            $('#btnId').click();
                            break;
                        case '4':
                            url = "/Admin/Cambiar_Status";
                            const parametros = {
                                user: $("#user").val(),
                            };
                            $.post(url, parametros, function (data) {
                                if (data.a == false) {
                                    Swal.fire({
                                        icon: 'error',
                                        text: data.b
                                    })
                                } else {
                                    Swal.fire({
                                        icon: 'success',
                                        text: data.b
                                    })
                                }

                            });
                            break;
                        case '5':
                            $('#ChangePass').click();
                            break;
                    }
                } else {
                    Swal.fire({
                        icon: 'error',
                        text: data.b
                    })
                }
            });
        } else if ((id != 1 && id != 6) && user == "") {
            Swal.fire({
                icon: 'error',
                text: 'Ingresa un Usuario.',
            })


        }
        if (id == 1) {
            document.location.href = "/Admin/AltaUser";
        }
        if (id == 6) {
            Swal.fire({
                icon: 'info',
                text: 'Se encuentra en configuracion!',
            })
        }

    });

    $("#user").keyup(function (e) {
        e.preventDefault();
        var user = $("#user").val();
        if (user != "") {
            url = "/Admin/BuscarLikeId";
            const postData = { user: user };
            $.post(url, postData, function (data) {
                if (data != "") {
                    let template = `<table class="table table-sm">\
                            <tbody><tr>\
                            <th>`+ data.a + `</th>\
                            <td>| `+ data.b + `</td>\
                            </tr>\
                            </tbody>\
                            <input type="hidden" id = "op" value = "`+ data.a + `" />\
                            </table >`;
                    $('#temp').html(template);

                } else {
                    let template = '';
                    $('#temp').html(template);
                }
            });
        } else {
            let template = '';
            $('#temp').html(template);
        }

    });
    $("#temp").click(function (e) {
        e.preventDefault();
        $("#user").val($("#op").val());
        let template = '';
        $('#temp').html(template);

    });
    $("#Fchange_pass").submit(function (e) {
        e.preventDefault();
        url = "/Admin/Cambiar_Pass";
        const parametros = {
            user: $("#user").val(),
            pass: $("#pass").val(),
            cpass: $("#Cpass").val()
        };
        $.post(url, parametros, function (data) {
            if (data.a == true) {
                Swal.fire({
                    icon: 'success',
                    text: data.b
                })
                $('#exampleModal').modal('hide');
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            } else {
                Swal.fire({
                    icon: 'error',
                    text: data.b
                })
            }
        });
    });




    $("#no_Licitacion").keyup(function (e) {
        e.preventDefault();
        var aux = $("#no_Licitacion").val();
        if (aux != "") {

            url = "/Constructora/BuscarLikeLicitacion";
            const postData = { nombre: aux };
            $.post(url, postData, function (data) {
                if (data.a == true) {
                    let template = "";
                    data.b.forEach(myFunction);
                    function myFunction(element, i) {
                        template += `
                    <div class="row" id="rowLic" onclick="Enviar('`+ element + `')" >
                    <div class="col-8">
                        <p>`+ element + `</p>
                    </div>
                    <div class="col-4 text-success">
                        <p>Aprobada</p>
                    </div>
                    </div>`;
                    }

                    $('#temp2').html(template);


                } else {
                    let template = '';
                    $('#temp2').html(template);
                }
            });
        } else {
            let template = '';
            $('#temp2').html(template);
        }

    });


    //JUVE


    $('.example').keyup(function (e) {
        e.preventDefault();
        example();
    });

    $('.example').on('change', function (e) {
        e.preventDefault();
        example();
    });


    function example() {

        if ($('#noLicitacion').val() == "" && $('#idMunicipio').val() == 0 && $('#Estatus').val() == 0) {
            $("body").load('#card-body');
        } else {
            var array = new Array(3);
            for (var i = 0; i < 3; i++) {
                array[i] = new Array(2);
            }
            var i = 0;
            $('.example').each(function () {
                if ($(this).val() != "" && $(this).val() != null) {
                    array[i][0] = $(this).attr('id');
                    array[i][1] = $(this).val();
                    i = i + 1;
                } else {
                    array[i][0] = $(this).attr('id');
                    array[i][1] = "";
                    i = i + 1;
                }
            });

            url = "/Constructora/MenuLikeProyectos";
            const postData = { array: array };
            $.post(url, postData, function (data) {
                if (data.a == true && data.b != "") {
                    let template = "";
                    var cont = 0;
                    data.b.forEach(myFunction);
                    function myFunction(element, i) {
                        if (i < 4) {
                            template += `
                          <div name="SelectProyecto" id="`+ element["idLicitacion"] + `" onclick="Seleccion(` + element["idLicitacion"] + `)">
                             <form action="/Constructora/VisualizarProyecto" method="post" id = "Form`+ element["idLicitacion"] + `">
                            
                                    <input type="hidden" name="idProyecto" value="`+ element["idProyecto"] + `"/>
                                    <input type="hidden" name="idLicitacion" value="`+ element["idLicitacion"] + `"/>
                                    <input type="submit" style="visibility:hidden;" />
                                </form>
                          <div class="row px-3 border border-left-0 border-right-0 border-bottom-0 border-primary">
                            <!--Inicia el primer renglon del registro-->
                          
                                <div class="col-3">
                                    <p>`+ element["noLicitacion"] + `</p><!--Tabla proyectos inner join licitaciones -> campo noLicitacion-->
                                </div>
                                <div class="col-2">
                                    <p>`+ element["Municipio"] + `</p><!--Tabla proyectos inner join licitaciones inner join catalogoMunicipio-> campo descripcion-->
                                </div>
                                <div class="col-2">
                                    <p>`+ element["Estatus"] + `</p><!--Tabla proyectos inner join licitaciones inner join catalogoEstatus-> campo descripcion-->
                                </div>
                                <div class="col-3">
                                    <p>`+ element["localidad"] + `</p><!--Tabla proyectos inner join licitaciones -> campo localidad-->
                                </div>
                                <div class="col-2">
                                    <p>`+ element["FCreacion"] + `</p><!--Tabla proyectos -> campo fechaCreacion-->
                                </div>  
                            </div><br><!--Termina el primer renglon del registro-->
                            <div class="row px-3">
                                <!--Inicio el segundo renglon del registro-->
                                <div class="col-8">
                                    <p><b>`+ element["nombreObra"] + `</b></p><!--Tabla proyectos inner join licitaciones -> campo nombreObre-->
                                </div>
                                <div class="col-4">
                                    <p>`+ element["Empresa"] + `</p><!--Tabla proyectos inner join licitaciones inner join catalogoEmpresas-> campo descripcion-->
                                </div>
                            </div><!--Termina el segundo renglon del registro-->
                    </div>`;
                        }
                    }

                    $('#card-body').html(template);


                } else {

                    $("#card-body").html('No existen Resultados');
                }
            });
        }
    }



    $("div[name=SelectProyecto]").click(function (e) {
        var temp = "Form";
        temp += $(this).attr('id');
        $('#' + temp).submit();
    });

    $('#btnNvoLic').on('click', function (e) {
        e.preventDefault();
        url = "/Constructora/BuscarLicitacion";
        const postData = { nombre: $("#no_Licitacion").val() };
        $.post(url, postData, function (data) {
            if (data.a == true) {
                $('#id_lic').val(data.b);
                $('#formNvoLic').submit();
            } else {
                Swal.fire({
                    icon: 'error',
                    text: data.b
                })
            }
        });
    });

    $('#LimpiarDocs').click(function (e) {
        e.preventDefault();
        $('#PolizaAnticipo').val('');
        $('#PolizaAnticipo').siblings(".custom-file-label").addClass("selected").html('PolizaAnticipo.pdf');
        $('#PolizaAnticipoCheck').prop("checked", false);
        $('#PolizaAnticipoCheck1').prop("checked", false);

        $('#PolizaCumplimiento').val('');
        $('#PolizaCumplimiento').siblings(".custom-file-label").addClass("selected").html('PolizaCumplimiento.pdf');
        $('#PolizaCumplimientoCheck').prop("checked", false);
        $('#PolizaCumplimientoCheck1').prop("checked", false);

        $('#PolizaVicios').val('');
        $('#PolizaVicios').siblings(".custom-file-label").addClass("selected").html('PolizaVicios.pdf');
        $('#PolizaViciosCheck').prop("checked", false);
        $('#PolizaViciosCheck1').prop("checked", false);

    });

    $('#BorrarDocs').click(function (e) {
        e.preventDefault();
        ($("#labelPC")[0].innerText);
        var array = [];
        if ($('#PolizaAnticipoCheck').prop("checked") && $("#labelPA")[0].innerText != "PolizaAnticipo.pdf") {
            array.push($("#labelPA")[0].innerText);
        }
        if ($('#PolizaCumplimientoCheck').prop("checked") && $("#labelPC")[0].innerText != "PolizaCumplimiento.pdf") {
            array.push($("#labelPC")[0].innerText);
        }
        if ($('#PolizaViciosCheck').prop("checked") && $("#labelPV")[0].innerText != "PolizaVicios.pdf") {
            array.push($("#labelPV")[0].innerText);
        }
        if (array.length != 0) {
            Swal.fire({
                title: 'Quieres borrar ' + array.length + ' documento(s)?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Si, borrarlos!'
            }).then((result) => {
                if (result.value) {
                    url = "/Constructora/BorrarDocumentos";
                    const postData = {
                        idProyecto: $("#idProyecto").val(),
                        array: array,
                    };
                    $.post(url, postData, function (data) {
                        if (data.a == true) {
                            if ($('#PolizaAnticipoCheck').prop("checked") && $("#labelPA")[0].innerText != "PolizaAnticipo.pdf") {
                                $('#PolizaAnticipo').val('');//vacio el input file
                                $('#PolizaAnticipo').siblings(".custom-file-label").addClass("selected").html('PolizaAnticipo.pdf'); //Se agrega el texto default del input
                                $("#labelPA").removeClass("text-success"); $("#labelPA").addClass("text-dark");
                                $('#PolizaAnticipoCheck').prop("checked", false); // Se deselecciona el check del model
                                $('#PolizaAnticipoCheck2').prop("checked", false);// Se deselecciona el check de la vista
                            }
                            if ($('#PolizaCumplimientoCheck').prop("checked") && $("#labelPC")[0].innerText != "PolizaCumplimiento.pdf") {
                                $('#PolizaCumplimiento').val('');
                                $('#PolizaCumplimiento').siblings(".custom-file-label").addClass("selected").html('PolizaCumplimiento.pdf');
                                $("#labelPC").removeClass("text-success"); $("#labelPC").addClass("text-dark");
                                $('#PolizaCumplimientoCheck').prop("checked", false);
                                $('#PolizaCumplimientoCheck2').prop("checked", false);
                            }
                            if ($('#PolizaViciosCheck').prop("checked") && $("#labelPV")[0].innerText != "PolizaVicios.pdf") {
                                $('#PolizaVicios').val('');
                                $('#PolizaVicios').siblings(".custom-file-label").addClass("selected").html('PolizaVicios.pdf');
                                $("#labelPV").removeClass("text-success"); $("#labelPV").addClass("text-dark");
                                $('#PolizaViciosCheck').prop("checked", false);
                                $('#PolizaViciosCheck2').prop("checked", false);
                            }
                            Swal.fire(
                                'Borrados!',
                                'Se borrarón con exito.',
                                'success'
                            )

                        } else {
                            Swal.fire({
                                icon: 'error',
                                text: data.b
                            })
                        }
                    });

                }
            })
        } else {

            Swal.fire({
                icon: 'error',
                text: "No existe nada cargado para borrar!"
            })
        }




    });



    //Para que aparezca el nombre del archivo que seleccionamos en el campo FILE
    $(".custom-file-input").on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).siblings(".custom-file-label").removeClass("text-dark");
        $(this).siblings(".custom-file-label").addClass("selected text-info").html(fileName);

    });


    CargarMunicipios();
    CargarEstatus();

});//----------------------------------------Fin del Document.Ready------------------------------------------

function Seleccion(i) {
    var temp = "Form";
    temp += i;
    $('#' + temp).submit();
}

function CargarEstatus() {
    url = "/Constructora/CargarEstatus";
    $.post(url, function (data) {
        if (data.a == true) {
            let template = "";

            data.b.forEach(myFunction);
            function myFunction(element, i) {
                var x = i + 4;
                if (i == 0) {
                    template += `
                     <option value="0" selected >TODOS</option>`;
                }
                template += `
                     <option value="`+ x + `" >` + element + `</option>`;
            }

            $('#Estatus').html(template);


        } else {
            let template = '';
            $('#Estatus').html(template);
        }
    });
}

function CargarMunicipios() {
    url = "/Constructora/BuscarMunicipios";
    $.post(url, function (data) {
        if (data.a == true) {
            let template = "";
            data.b.forEach(myFunction);
            function myFunction(element, i) {
                if (i == 0) {
                    template += `
                     <option value="0" selected >MUNICIPIOS</option>`;
                }
                template += `
                     <option value="`+ (i + 1) + `" >` + element + `</option>`;
            }

            $('#idMunicipio').html(template);


        } else {
            let template = '';
            $('#idMunicipio').html(template);
        }
    });


}

function Enviar(i) {

    $("#no_Licitacion").val(i);
    let template = '';
    $('#temp2').html(template);
}


$("div[name=SelectProyecto]").click(function (e) {
    var temp = "Form";
    temp += $(this).attr('id');
    $('#' + temp).submit();
});