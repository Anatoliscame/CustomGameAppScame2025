function apriOrderAcquisto(formContext) {

    var statuscodeVG = formContext.getAttribute("statuscode").getValue();
    if (statuscodeVG !== 746200003) { Xrm.Navigation.openAlertDialog({ text: "Video Game non acquistabile" }); return; }

    var typePiattaformaVG = formContext.getAttribute("acn_typepiattaforma").getValue();
    if (typePiattaformaVG === 746200006) { Xrm.Navigation.openAlertDialog({ text: "Scegli prima una piattaforma d'acquistare" }); return; }


    if (formContext.data.entity.getEntityName() !== "acn_videogame") {
        alert("Questo pulsante funziona solo su VideoGame.");
        return;
    }

    var videoGameId = formContext.data.entity.getId();
    videoGameId = videoGameId.replace("{", "").replace("}", "");
    var videoGameName = formContext.getAttribute("acn_key")?.getValue(); // Nome di VideoGame
    if (!videoGameName) {
        alert("Il nome del videogioco non è disponibile.");
        return;
    }

    var account = formContext.getAttribute("acn_accountid")?.getValue();
    if (!account || account.length === 0) {
        alert("Account non selezionato sul VideoGame.");
        return;
    }
    var accountId = account[0].id.replace("{", "").replace("}", "");

    function generaNumeroCasuale() {
        return Math.floor(Math.random() * 1000000) + 100000;
    }

    var newOrder = {
        "acn_ordername": "Order-" + generaNumeroCasuale(),
        "acn_videogameid@odata.bind": "/acn_videogames(" + videoGameId + ")",
        "acn_accountid@odata.bind": "/accounts(" + accountId + ")"
    };
    var tipoVideogiocoParent = formContext.getAttribute("acn_tipovideogioco").getValue();
    if (tipoVideogiocoParent !== null && typeof tipoVideogiocoParent !== "undefined") {

        var num = CheckExistKeGameInVideoGame(videoGameId, typePiattaformaVG);
        if (num === 2) {
            if (tipoVideogiocoParent !== 746200003) { //Espansione

                if (tipoVideogiocoParent === 746200000 || tipoVideogiocoParent === 746200001) { // Base Game o DLC
                    creaOrdineAcquisto(newOrder);
                }
            }
        } else if (num === 1) {
            // Nessuna chiave per la piattaforma richiesta
            Xrm.Navigation.openAlertDialog({ text: "Non ci sono chiavi disponibili per questa piattaforma." });
        } else if (num === -1) {
            Xrm.Navigation.openAlertDialog({ text: "Chiavi disponibili con un video gioco non ci sono." });
        } else if (num === -2) {
            // Errore nella chiamata HTTP
            Xrm.Navigation.openAlertDialog({ text: "Errore durante la verifica delle chiavi." });
        }

        if (tipoVideogiocoParent === 746200003) { // Espansione
            CheckExistParentChildVideoGame(videoGameId, typePiattaformaVG, statuscodeVG, tipoVideogiocoParent, newOrder);
        }
    }
}

function CheckExistParentChildVideoGame(videoGameId, typePiattaformaVG, statuscodeVG,tipoVideogiocoParent, newOrder) {


    var fetchUrl = "<fetch mapping='logical' version='1.0' output-format='xml-platform' distinct='false' >" +
        "<entity name='acn_videogame'>" +
        "<filter type='and'>" +
        "<condition attribute='acn_parentvideogameid' operator='eq' value='" + videoGameId + "' />" +
        "</filter>" +
        "<attribute name='acn_videogameid' />" +
        "<attribute name='acn_typepiattaforma' />" +
        "<attribute name='statuscode' />" +
        "</entity>" +
        "</fetch>";
    var path = "?fetchXml=" + encodeURIComponent(fetchUrl);


    Xrm.WebApi.retrieveMultipleRecords("acn_videogame", path).then(
        function success(result) {
            if (result.entities.length > 0) {
                var estensionVStatus = false;
                var estensionVTypePiattaf = false;
                for (var i = 0; i < result.entities.length; i++) {
                    var videoGames = result.entities[i];
                    var statuscodeParentChild = videoGames.statuscode;
                    var typepiattaformaParentChild = videoGames.acn_typepiattaforma;
                    if (statuscodeParentChild !== statuscodeVG) {
                        estensionVStatus = true;
                        break;
                    }
                    if (typepiattaformaParentChild !== typePiattaformaVG) {
                        estensionVTypePiattaf = true;
                        break;
                    }
                }
                if (estensionVStatus) {
                    Xrm.Navigation.openAlertDialog({
                        text: "I contenuti di epsansioni o uno solo non e' presente nella 'DISPONIBILITA'."
                    });
                    return;

                } else if (estensionVTypePiattaf) {
                    Xrm.Navigation.openAlertDialog({
                        text: "Piattaforma non corrispondente o mancante per le espansioni di Parent Child."
                    });
                    return;

                } else {
                   // CheckExistKeGameInVideoGame(videoGameId, typePiattaformaVG, tipoVideogiocoParent, newOrder);
                    creaOrdineAcquisto(newOrder);
                }

            } else {
                console.log("Nessuna espansione trovata.");
            }
        },
        function (error) {
            console.error("Errore fetch espansioni: " + error.message);
        }
    );
}

function CheckExistKeGameInVideoGame(videoGameId, typePiattaformaVG) {

    var num = 0;
    var req = new XMLHttpRequest();
    req.open("GET", Xrm.Utility.getGlobalContext().getClientUrl() + "/api/data/v9.2/acn_keygames?$select=acn_typepiattaforma&$filter=(_acn_videogame_value eq " + videoGameId + " and acn_statuspresentkeygame eq 746200000)", false);
    req.setRequestHeader("OData-MaxVersion", "4.0");
    req.setRequestHeader("OData-Version", "4.0");
    req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    req.setRequestHeader("Accept", "application/json");
    req.setRequestHeader("Prefer", "odata.include-annotations=*");
    req.onreadystatechange = function () {
        if (this.readyState === 4) {
            req.onreadystatechange = null;
            if (this.status === 200) {
                var results = JSON.parse(this.response);
                console.log(results);
                if (results.value.length <= 0) {
                    //Xrm.Navigation.openAlertDialog({ text: "keyGameArray: Chiavi disponibili con un video gioco non ci sono." });
                    num = -1;
                } else {
                    var keyTrovata = false;
                    for (var i = 0; i < results.value.length; i++) {
                        var keyGame = results.value[i];
                        if (keyGame["acn_typepiattaforma"] === typePiattaformaVG) {
                            keyTrovata = true;
                            break;
                        }
                    }
                    if (!keyTrovata) {
                        num = 1; // //Xrm.Navigation.openAlertDialog({ text: "Non ci sono chiavi disponibili per questa piattaforma." });
                    } else {
                        num = 2; // Chiave trovata
                    }
                }

            } else {
                console.log(this.responseText);
                num = -2;
            }
        }
    };
    req.send();
    return num;
}

function creaOrdineAcquisto(newOrder) {

    // Step 1: crea OrderAcquisto
    Xrm.WebApi.createRecord("acn_ordineacquisto", newOrder).then(
        function (result) {
            var orderId = result.id;

            // Step 2: recupera l’OrderAcquisto appena creato (con il campo acn_acquistoid popolato dal plugin)
            Xrm.WebApi.retrieveRecord("acn_ordineacquisto", orderId, "?$select=acn_ordineacquistoid&$expand=acn_acquistoid($select=acn_acquistoid)").then(
                function (order) {
                    if (order.acn_acquistoid && order.acn_acquistoid.acn_acquistoid) {
                        var acquistoId = order.acn_acquistoid.acn_acquistoid;

                        // Step 3: naviga verso la pagina dell’Acquisto
                        Xrm.Navigation.openForm({
                            entityName: "acn_acquisto",
                            entityId: acquistoId
                        });
                        Xrm.Navigation.openAlertDialog({ text: "Ordine creato" });

                    } else {
                        Xrm.Navigation.openAlertDialog({ text: "Ordine creato, ma non è stato possibile identificare l'Acquisto associato." });
                    }
                },
                function (error) {
                    console.error("Errore nel recupero dell’OrderAcquisto: " + error.message);
                }
            );
        },
        function (error) {
            console.error("Errore nella creazione dell'OrderAcquisto: " + error.message);
        }
    );
}