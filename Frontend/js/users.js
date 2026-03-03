function initialize() {
  let addBtn = document.querySelector("#addBtn")
  addBtn.addEventListener("click", function () {
    window.location.href = 'userForm.html'
  })

  getAll()
}

function getAll() {
    const params = new URLSearchParams(window.location.search);
    const groupId = params.get('id');

    // sa id trazimo clanove grupe, bez id trazimo sve
    const url = groupId ? `http://localhost:51509/api/groups/${groupId}/users` : 'http://localhost:51509/api/users';

    fetch(url) 
        .then(response => {
            if (!response.ok) {
                throw new Error('Request failed. Status: ' + response.status);
            }
            return response.json();
        })
        .then(users => {
            renderData(users); 

            if (groupId) {
                ucitajKorisnikeZaDodavanje(groupId);
            }
        })       
        .catch(error => {                        
            console.error('Error:', error.message);
            let table = document.querySelector('.table-details table');
            if (table) {
                table.style.display = 'none';
            }
            alert('An error occurred while loading the data.');
        });
}

function renderData(data) {
    let table = document.querySelector('table tbody');
    table.innerHTML = '';
    let tableHeader = document.querySelector('table thead');
    let noDataMessage = document.querySelector('#no-data-message');

    const gId = new URLSearchParams(window.location.search).get('id');

    let membershipSection = document.getElementById('membership-management');
    let thUkloni = document.querySelector('table thead th:nth-child(6)');
    if (gId) {// ako ima id, pokazi sekciju za clanove i "ukloni" kolonu
        
        if (membershipSection) membershipSection.style.display = 'block';
        if (thUkloni) thUkloni.style.display = ''; 
    } else {
        // ako nema id, sakri sve i "ukloni" kolonu
        if (membershipSection) membershipSection.style.display = 'none';
        if (thUkloni) thUkloni.style.display = 'none';
    }

    if (!data || data.length === 0) {
        tableHeader.classList.add('hidden');
        noDataMessage.style.display = 'block';
        noDataMessage.classList.remove('hidden');
    } else {
        noDataMessage.style.display = 'none';
        noDataMessage.classList.add('hidden');
        tableHeader.classList.remove('hidden');

        data.forEach(user => {
            let newRow = document.createElement('tr');

            let cell1 = document.createElement('td');
            cell1.textContent = user['korisnickoIme'];
            newRow.appendChild(cell1);

            let cell2 = document.createElement('td');
            cell2.textContent = user['ime'];
            newRow.appendChild(cell2);

            let cell3 = document.createElement('td');
            cell3.textContent = user['prezime'];
            newRow.appendChild(cell3);

            let cell4 = document.createElement('td');
            cell4.textContent = user['datumRodjenja'] ? new Date(user['datumRodjenja']).toLocaleDateString('sr-RS') : '/';
            newRow.appendChild(cell4);

            let cell5 = document.createElement('td');
            let editButton = document.createElement('button');
            editButton.textContent = 'Edit';
            editButton.className = 'btn-edit'; //boja dugmeta
            editButton.onclick = function () {
                window.location.href = 'userForm.html?id=' + user['id'];
            };
            cell5.appendChild(editButton);
            newRow.appendChild(cell5);

            
            if (gId) { // remove dugme celija, jedino u grupi
                let cell6 = document.createElement('td');
                let removeBtn = document.createElement('button');
                removeBtn.textContent = 'Ukloni';
                removeBtn.className = 'btn-delete';
                removeBtn.onclick = function() {
                    ukloniIzGrupe(user.id, gId); 
                };
                cell6.appendChild(removeBtn);
                newRow.appendChild(cell6);
            }

            table.appendChild(newRow);
        });
    }
}

document.addEventListener('DOMContentLoaded', initialize)

function ucitajKorisnikeZaDodavanje(groupId) {
    fetch('http://localhost:51509/api/users')
        .then(resSvi => resSvi.json())
        .then(sviKorisnici => {
            fetch(`http://localhost:51509/api/groups/${groupId}/users`)
                .then(resClanovi => resClanovi.json())
                .then(clanoviGrupe => {
                    const tbody = document.getElementById('tbody-non-members');
                    tbody.innerHTML = '';

                    const vanGrupe = sviKorisnici.filter(s => 
                        !clanoviGrupe.some(c => c.id === s.id)
                    );

                    vanGrupe.forEach(u => {
                        let row = document.createElement('tr');
                        row.innerHTML = `
                            <td>${u.ime} ${u.prezime}</td>
                            <td>${u.korisnickoIme}</td>
                            <td><button class="btn-add" onclick="dodajUGrupu(${u.id}, ${groupId})">Dodaj</button></td>
                        `;
                        tbody.appendChild(row);
                    });
                });
        })
        .catch(err => console.error("Greška:", err));
}

function dodajUGrupu(uId, gId) {
    fetch(`http://localhost:51509/api/groups/${gId}/users/${uId}`, { 
        method: 'PUT'
    })
    .then(res => { 
        if(res.ok) {
            location.reload();
        } else {
            alert("Greška pri dodavanju. Status: " + res.status);
        }
    })
    .catch(err => console.error("Greška:", err));
}

function ukloniIzGrupe(uId, gId) {
    if(confirm("Da li ste sigurni da želite da uklonite korisnika iz grupe?")) {
        fetch(`http://localhost:51509/api/groups/${gId}/users/${uId}`, { 
            method: 'DELETE' 
        })
        .then(res => { 
            if(res.ok) {
                location.reload(); 
            } else {
                alert("Greška pri uklanjanju korisnika.");
            }
        });
    }
}