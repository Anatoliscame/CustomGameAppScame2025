/*
Ho un problema nel raggionare.Ho entita su CRM Dynamics 365,
    1. Account relazionato con Acquisto, ha tanti acquisti in un grid.Account sono le persone che possono fare acquisti di video giochi.
2. Acquisto e' relazionato con Account e OrderAcquisto. Acquisto ha un elenco di OrderAcquisto
3. OrderAcquisto e' relazionato con Acquisto, VideoGame. Ogni OrderAcquisto ha un VideoGame e Acquisto.
4. VideoGame e' relazionato con KeyGame, un videogame puo avere un'elenco di KeyGame.Un VideoGame e' un video gioco, un video gioco e' presente nel shopGame se i KeyGame sono disponibili, se non sono disponibili  un video gioco deve essere considerato come terminato.
5. KeyGame ha un VideoGame, KeyGame(sarebbe nome).Un KeyGame significa una chiave per attivazione per gli account steam o psn.

Mi interessa di sapere come organizzare la logica, esempio: una persona(Account) puo scegliere selezionando piu di un video gioco dentro(OrderAcquisto) prima di fare Acquisto, una volta il cliente o persona ha finito, deve procedere con l'acquisto. Puo decidere acquistare anche un'altro giorno e quindi acquisto deve rimanere inattesa altrimenti acquisto effettuato. 
Mi serve perche confondo entita Acquisto con un carrello o con una fattura che arriva in email con la chiave.
Ho creato una logica, un plugin per OrderAcquisto, prima che viene creato OrderAcquisto, nel plugin va a controllare se il numero di KeyGame riferito al VideoGame sono ancora disponibili, ricordando che VideoGame viene inserita all'interno OrderAcquisto



Non riesco testare in locale crm, perche quando uso ribbon bottone di customer(update), dopo alla sua volta deve essere eseguito il plugin.Il fatto  che funziona tutto bene, mentre se provo lanciare in debbug, risulta null


autenticazione e autorizzazione


acn_kestatusacquisto

statuscode



1.Il plug -in "OnPreCreateUpdateBudgetCheckAssignedBudget" deve essere eseguito durante "pre-operation (Create) e pre-operation (Update)", usando l'entita  entita keye_budget
2. Nel Update "pre-operation (Update)", deve essere trigerato il campo "Assigned Budget -> (keye_amount)".
3. Controllare se il campo "keye_amount" e' vuoto 



Mi servi, per aggiungere idee nuovi per l'app, esempio vorrei fare una soluzione di "parent" nell'app, ma non so dove posso applicare

*/