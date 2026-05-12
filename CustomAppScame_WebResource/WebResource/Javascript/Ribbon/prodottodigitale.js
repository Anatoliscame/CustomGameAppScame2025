function apriOrderAcquisto(formContext) {

    var statusPD = formContext.getAttribute("sc_statoprodottodigitale").getValue();
    if (statusPD !== 126400000) { Xrm.Navigation.openAlertDialog({ text: "Prodotto Digitale non acquistabile" }); return; }


    var typePiattaforma = formContext.getAttribute("sc_piattaformaprodotttodigitale").getValue();

    if (typePiattaforma === null) {
        Xrm.Navigation.openAlertDialog({
            text: "Scegli prima una piattaforma d'acquistare"
        });
        return;
    }

    if (formContext.data.entity.getEntityName() !== "sc_prodottodigitale") {
        alert("Questo pulsante funziona solo su Prodottto Digitale.");
        return;
    }

    var prodottoDigitaleId = formContext.data.entity.getId();
    prodottoDigitaleId = prodottoDigitaleId.replace("{", "").replace("}", "");


    var prodottoDigitaleName = formContext.getAttribute("sc_key")?.getValue(); // Name di prodotto Digitale
    if (!prodottoDigitaleName) {
        alert("Il nome del prodotto Digitale non è disponibile.");
        return;
    }

    var account = formContext.getAttribute("sc_accountcliente")?.getValue();
    if (!account || account.length === 0) {
        alert("Account non selezionato sul Prodottto Digitale.");
        return;
    }
    var accountId = account[0].id.replace("{", "").replace("}", "");
    function generaNumeroCasuale() {
        return Math.floor(Math.random() * 1000000) + 100000;
    }

    var newOrder = {
        "sc_name": "Order-" + generaNumeroCasuale(),
        "sc_prodottodigitaleid@odata.bind": "/sc_prodottodigitales(" + prodottoDigitaleId + ")",
        "sc_accountcliente@odata.bind": "/accounts(" + accountId + ")"
    };

    var valueTypeExpOrLicSoft = RetriveValueTypeExpOrLicSoftCountry(prodottoDigitaleId);

    if (valueTypeExpOrLicSoft === null || typeof valueTypeExpOrLicSoft === "undefined") {
        return;
    }
    var tipoProductAttr = formContext.getAttribute("sc_tipoprodottodigitale");
 
    var num = CheckExistKeyProduct(prodottoDigitaleId, typePiattaforma);
    if (num === 2) { // Chiavi di prodotto digitale DISPONIBILI

        if (tipoProductAttr.getValue() == 126400000) // VideoGame
        {

            if (valueTypeExpOrLicSoft !== 126400003) { // diverso da Espansione

                if (valueTypeExpOrLicSoft === 126400000 || valueTypeExpOrLicSoft === 126400002)
                { // Base Game o DLC e Remastered
                    creaOrdineAcquisto(newOrder);
                    Xrm.Navigation.openAlertDialog({ text: "Base Game o Remastered" });
                }else
                if (valueTypeExpOrLicSoft === 126400001)
                {
                    var parentprodottodigitaleid = formContext.getAttribute("sc_parentprodottodigitaleid") != null
                        ? formContext.getAttribute("sc_parentprodottodigitaleid").getValue()
                        : null;
                    if (parentprodottodigitaleid == null) {
                        Xrm.Navigation.openAlertDialog({ text: "Il valore non e' stato impostato di prodotto digitale di parent e di  DLC" });
                        return; 
                    } else {
                        creaOrdineAcquisto(newOrder);
                        Xrm.Navigation.openAlertDialog({ text: "DLC" });
                    }
                }
            } else {

                // Espansione
                Xrm.Navigation.openAlertDialog({ text: "Procedi con l'espansione." });
                CheckExistParentChildProdottoDigitale(prodottoDigitaleId, typePiattaforma, statusPD, newOrder);
            }
        }
        if (tipoProductAttr.getValue() == 126400001) // Licenza Software
        {
            creaOrdineAcquisto(newOrder);
            Xrm.Navigation.openAlertDialog({ text: "Licenza Software" });
        }

    } else if (num === 1) {
        // Nessuna chiave per la piattaforma richiesta
        Xrm.Navigation.openAlertDialog({ text: "Non ci sono chiavi disponibili per questa piattaforma." });
    } else if (num === -1) {
        Xrm.Navigation.openAlertDialog({ text: "Chiavi disponibili con un prodotto digitale non ci sono." });
    } else if (num === -2) {
        // Errore nella chiamata HTTP
        Xrm.Navigation.openAlertDialog({ text: "Errore durante la verifica delle chiavi." });
    }
}



// Video Game Parent Child

function CheckExistParentChildProdottoDigitale(prodottoDigitaleId, typePiattaforma, statusPD, newOrder) {


    var fetchUrl = "<fetch mapping='logical' version='1.0' output-format='xml-platform' distinct='false' >" +
        "<entity name='sc_prodottodigitale'>" +
        "<filter type='and'>" +
        "<condition attribute='sc_parentprodottodigitaleid' operator='eq' value='" + prodottoDigitaleId + "' />" +
        "</filter>" +
        "<attribute name='sc_prodottodigitaleid' />" +
        "<attribute name='sc_piattaformaprodotttodigitale' />" +
        "<attribute name='sc_statoprodottodigitale' />" +
        "</entity>" +
        "</fetch>";
    var path = "?fetchXml=" + encodeURIComponent(fetchUrl);


    Xrm.WebApi.retrieveMultipleRecords("sc_prodottodigitale", path).then(
        function success(result) {
            if (result.entities.length > 0) {
                var estensionPDStatus = false;
                var estensionPDTypePiattaf = false;

                for (var i = 0; i < result.entities.length; i++) {
                    var prodotiDigitale = result.entities[i];

                    var statuscodeParentChild = prodotiDigitale.sc_statoprodottodigitale;
                    var typepiattaformaParentChild = prodotiDigitale.sc_piattaformaprodotttodigitale;
                    var idV = prodotiDigitale.sc_prodottodigitaleid;

                    if (statuscodeParentChild !== statusPD) {
                        estensionPDStatus = true;
                        break;
                    }
                    if (typepiattaformaParentChild !== typePiattaforma) {
                        estensionPDTypePiattaf = true;
                        break;
                    }
                }

                if (estensionPDStatus) {
                    Xrm.Navigation.openAlertDialog({
                        text: "I contenuti di epsansioni o uno solo non e' presente nella 'DISPONIBILITA'."
                    });
                    return;

                } else if (estensionPDTypePiattaf) {
                    Xrm.Navigation.openAlertDialog({
                        text: "Piattaforma non corrispondente o mancante per le espansioni di Parent Child."
                    });
                    return;

                } else {

                    var numV = CheckExistKeyProduct(idV, typepiattaformaParentChild);

                    if (numV === 2) {
                        creaOrdineAcquisto(newOrder);
                        Xrm.Navigation.openAlertDialog({ text: "creaOrdineAcquisto e' stato creato" });
                    } else if (numV === -1) {
                        Xrm.Navigation.openAlertDialog({ text: "CHILD: Chiavi disponibili con un video game non ci sono." });
                    } else if (numV === -2) {
                        // Errore nella chiamata HTTP
                        Xrm.Navigation.openAlertDialog({ text: "CHILD: Errore durante la verifica delle chiavi." });
                    }
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

function RetriveValueTypeExpOrLicSoftCountry(prodottoDigitaleId) {

    var valueTypeExpOrLicSoft = null;
    var valueCountry = false;

    var req = new XMLHttpRequest();
    req.open("GET", Xrm.Utility.getGlobalContext().getClientUrl() + "/api/data/v9.2/sc_prodottodigitales(" + prodottoDigitaleId + ")?$select=sc_prodottodigitaleid&$expand=sc_productdetails($select=sc_typeexpansion,sc_tipolicenza,_sc_country_value)", false);
    req.setRequestHeader("OData-MaxVersion", "4.0");
    req.setRequestHeader("OData-Version", "4.0");
    req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    req.setRequestHeader("Accept", "application/json");
    req.setRequestHeader("Prefer", "odata.include-annotations=*");
    req.onreadystatechange = function () {
        if (this.readyState === 4) {
            req.onreadystatechange = null;
            if (this.status === 200) {
                var resultProdottoDigitale = JSON.parse(this.response);

                if (resultProdottoDigitale.sc_productdetails != null) {

                    var productDetails = resultProdottoDigitale.sc_productdetails;

                    var tipoPDParent = productDetails["sc_typeexpansion"];
                    var tipoLicenzaSoftParent = productDetails["sc_tipolicenza"];
                    var country = productDetails["_sc_country_value"];
                    
                    if (tipoPDParent !== null && typeof tipoPDParent !== "undefined") {

                        Xrm.Navigation.openAlertDialog({title: "Type Expansion",text: "Valore sc_typeexpansion: " + tipoPDParent});
                        valueTypeExpOrLicSoft = tipoPDParent;
                    }
                    else if (tipoLicenzaSoftParent !== null && typeof tipoLicenzaSoftParent !== "undefined")
                    {
                        Xrm.Navigation.openAlertDialog({title: "Tipo di Licenza Software",text: "Valore sc_tipolicenza: " + tipoLicenzaSoftParent});
                        valueTypeExpOrLicSoft = tipoLicenzaSoftParent;
                    }
                    else {
                        Xrm.Navigation.openAlertDialog({text: "Il campo sc_tipolicenza è vuoto o sc_typeexpansion."});
                    }
                    if (country !== null && typeof country !== "undefined") {
                        Xrm.Navigation.openAlertDialog({title: "country",text: "Country e' valorizzato: " + country});
                        valueCountry = true;
                    } else {
                        Xrm.Navigation.openAlertDialog({text: "Il campo country è vuoto."});
                    }

                    if (valueCountry === false) { return null;}

                } else {
                    Xrm.Navigation.openAlertDialog({text: "Product Details non è collegato al Prodotto Digitale."});
                }
            } else {
                console.log(this.responseText);
            }
        }
    };
    req.send();
    return valueTypeExpOrLicSoft;
}



function CheckExistKeyProduct(prodottoDigitaleId, typePiattaforma) {

    var num = 0;
    var req = new XMLHttpRequest();
    req.open("GET", Xrm.Utility.getGlobalContext().getClientUrl() + "/api/data/v9.2/sc_keyprodottos?$select=_sc_prodottodigitaleid_value,sc_typepiattaforma&$filter=_sc_prodottodigitaleid_value eq " + prodottoDigitaleId + "  and sc_statuspresentkey eq 126400000", false); // Disponibile
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
                    num = -1;
                } else {
                    var keyTrovata = false;
                    for (var i = 0; i < results.value.length; i++) {
                        var keyGame = results.value[i];
                        if (keyGame["sc_typepiattaforma"] === typePiattaforma) {
                            keyTrovata = true;
                            break;
                        }
                    }
                    if (!keyTrovata) {
                        num = 1;
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
    Xrm.WebApi.createRecord("sc_ordineacquisto", newOrder).then(
        function (result) {
            var orderId = result.id;
            // Step 2: recupera l’OrderAcquisto appena creato (con il campo acn_acquistoid popolato dal plugin)
            Xrm.WebApi.retrieveRecord("sc_ordineacquisto", orderId, "?$select=sc_ordineacquistoid,_sc_acquisto_value&$expand=sc_acquisto($select=sc_acquistoid)").then(
                function (order) {
                    //if (order["_sc_acquisto_value"]) {
                        //var acquistoId = order["_sc_acquisto_value"];
                    if (order.sc_acquisto && order.sc_acquisto.sc_acquistoid) {
                        var acquistoId = order.sc_acquisto.sc_acquistoid;
                        // Step 3: naviga verso la pagina dell’Acquisto
                        Xrm.Navigation.openForm({
                            entityName: "sc_acquisto",
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