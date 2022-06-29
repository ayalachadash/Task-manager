const url = '/User';

const login = () => {
    const addUserName = document.getElementById('add-userName');
    const addPassword = document.getElementById('add-password');

    const user = {
        UserName: addUserName.value.trim(),
        Password: addPassword.value.trim()
    }

    fetch(`${url}/Login`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    })
        .then(response => response.json())
        .then((data) => {
            addUserName.value = '';
            addPassword.value = '';
            localStorage.setItem("token", data);
            location.href = "/edit.html";
        })
        .catch(error => {
            addUserName.value = '';
            addPassword.value = '';
            console.error(alert("user is not found"));
        });
}