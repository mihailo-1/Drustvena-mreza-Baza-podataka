function initializeForm() {

  let submitBtn = document.querySelector("#submitBtn")
  submitBtn.addEventListener("click", submit)

  let cancelBtn = document.querySelector("#cancelBtn")
  cancelBtn.addEventListener("click", function () {
    window.location.href = 'users.html'
  })

  const urlParams = new URLSearchParams(window.location.search)
  const id = urlParams.get('id') 

  if (!id) { 
    document.getElementById('addTitle').style.display = 'block';
    document.getElementById('editTitle').style.display = 'none';
    return
  }

  document.getElementById('addTitle').style.display = 'none';
  document.getElementById('editTitle').style.display = 'block';

  get(id)
}

function get(id) {

  fetch('http://localhost:51509/api/users/' + id)
    .then(response => {
      if (!response.ok) {
        
        const error = new Error('Request failed. Status: ' + response.status)
        error.response = response 
        throw error  
      }
      return response.json()
    })
    .then(user => { 
      document.querySelector('#username').value = user.korisnickoIme
      document.querySelector('#firstName').value = user.ime
      document.querySelector('#lastName').value = user.prezime
      const formatted = new Date(user.datumRodjenja).toISOString().split('T')[0]; 
      document.querySelector('#dateOfBirth').value = formatted
    })
    .catch(error => {
      console.error('Error:', error.message)
      if (error.response && error.response.status === 404) {
        alert('user does not exist!')
      } else {
        alert('An error occurred while loading the data. Please try again.')
      }
    })
}

function submit() {
  
  document.querySelectorAll("[id$='Error']").forEach(el => el.textContent = '');

  const form = document.querySelector('#form');
  if (!form) {
    alert('Forma nije pronadjena');
    return;
  }

  const formData = new FormData(form);

  
  const podaci = {
    korisnickoIme:   formData.get('username')?.trim()    || '',
    ime:             formData.get('firstName')?.trim()   || '',
    prezime:         formData.get('lastName')?.trim()    || '',
    datumRodjenja:   formData.get('dateOfBirth') 
                       ? new Date(formData.get('dateOfBirth')).toISOString()
                       : null
  };

  
  if (!podaci.korisnickoIme) {
    document.querySelector('#usernameError').textContent = 'Korisnicko ime je obavezno';
    return;
  }
  if (!podaci.ime) {
    document.querySelector('#firstNameError').textContent = 'Ime je obavezno';
    return;
  }
  if (!podaci.prezime) {
    document.querySelector('#lastNameError').textContent = 'Prezime je obavezno';
    return;
  }
  if (!podaci.datumRodjenja) {
    document.querySelector('#dateOfBirthError').textContent = 'Datum rodjenja je obavezan';
    return;
  }

  
  const urlParams = new URLSearchParams(window.location.search);
  const id = urlParams.get('id');

  const method = id ? 'PUT' : 'POST';
  const url = id 
    ? `http://localhost:51509/api/users/${id}`
    : 'http://localhost:51509/api/users';


  
  fetch(url, {
    method: method,
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(podaci)
  })
  .then(async response => {
    console.log('Status odgovora:', response.status);

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`Greška ${response.status}: ${errorText}`);
    }

    return response.text(); 
  })
  .then(() => {
    console.log('Uspešno sačuvano');
    window.location.href = 'users.html';
  })
  .catch(err => {
    console.error('Greška pri submitu:', err);
    alert('Neuspešno čuvanje:\n' + err.message);
  });
}

document.addEventListener('DOMContentLoaded', initializeForm)