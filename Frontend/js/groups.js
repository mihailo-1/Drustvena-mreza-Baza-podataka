function ucitajGrupe() {
    fetch("http://localhost:51509/api/groups")
        .then(response => {
            if (!response.ok) {
                throw new Error("Mrežna greška");
            }
            return response.json();
        })
        .then(data => {
            prikaziGrupe(data);
        })
        .catch(error => {
            console.error("Greška:", error);
            document.getElementById("status-poruka").innerText = "Greška pri učitavanju.";
        });
}

function prikaziGrupe(grupe) {
    const tbody = document.getElementById("groups-table-body");
    tbody.innerHTML = "";

    grupe.forEach(g => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${g.id}</td>
            <td>${g.ime}</td> 
            <td>
                <button class="btn-members" onclick="window.location.href='users.html?id=${g.id}'">Članovi</button>
                
                <button class="btn-delete-group" onclick="obrisiGrupu(${g.id})">Obriši</button>
            </td>
        `;
        tbody.appendChild(tr);
    });
}

function obrisiGrupu(id) {
    if (confirm("Da li ste sigurni?")) {
        fetch(`http://localhost:51509/api/groups/${id}`, {
            method: 'DELETE'
        })
        .then(response => {
            if (response.ok) {
                ucitajGrupe(); 
            } else {
                alert("Brisanje nije uspelo.");
            }
        })
        .catch(error => console.error("Greška pri brisanju:", error));
    }
}

function dodajGrupu() {
    const nazivInput = document.getElementById("naziv-nove-grupe");
    const naziv = nazivInput.value;
    const poruka = document.getElementById("poruka-dodavanje");

    if (!naziv.trim()) {
        alert("Naziv ne može biti prazan!");
        return;
    }

    const novaGrupa = {
        ime: naziv
    };

    fetch("http://localhost:51509/api/groups", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(novaGrupa)
    })
    .then(response => {
        if (response.ok) {
            poruka.innerText = "Uspešno ste dodali grupu!";
            poruka.style.color = "green";
            nazivInput.value = "";
            ucitajGrupe();
        } else {
            poruka.innerText = "Greška prilikom čuvanja na serveru.";
            poruka.style.color = "red";
        }
    })
    .catch(err => {
        console.error("Greška pri POST zahtevu:", err);
        poruka.innerText = "Server nije dostupan.";
    });
}

document.getElementById("btn-sacuvaj-grupu").addEventListener("click", function() {
    dodajGrupu();
});

ucitajGrupe();