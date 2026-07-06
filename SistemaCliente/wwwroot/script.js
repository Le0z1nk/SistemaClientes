const API = "https://localhost:7221/api/clientes"

async function cadastrar() {

    const cliente = {
        codigo: document.getElementById("cadCodigo").value,
        nome: document.getElementById("cadNome").value,
        telefone: document.getElementById("cadTelefone").value,
        email: document.getElementById("cadEmail").value
    };

    const response = await fetch(API, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(cliente)
    });

    const dados = await response.json();
    alert(JSON.stringify(dados, null, 2));
}

async function consultar() {
    const codigo = document.getElementById("conCodigo").value;
    const response = await fetch(`${API}/${codigo}`);
    const cliente = await response.json();
    document.getElementById("resultadoConsulta").textContent = JSON.stringify(cliente, null, 4);
}

async function atualizar() {
    const codigo = document.getElementById("altCodigo").value;
    const contato = {
        nome: document.getElementById("altNome").value,
        telefone: document.getElementById("altTelefone").value,
        email: document.getElementById("altEmail").value
    };

    const response = await fetch(`${API}/${codigo}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(contato)
    });

    const dados = await response.json();
    alert(JSON.stringify(dados, null, 2));

}