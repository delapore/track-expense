const uri = 'http://localhost:5000/api/v1/expenses';
let expenses = [];

function getItems() {
  fetch(uri)
    .then(response => {
      if (!response.ok) {
        throw new Error(response.status);
      }
      return response.json();
    })
    .then(data => _displayItems(data))
    .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
  const addDateDateBox = document.getElementById('add-date');
  const addTypeListBox = document.getElementById('add-type');
  const addRecipientTextbox = document.getElementById('add-recipient');
  const addAmountTextBox = document.getElementById('add-amount');
  const addCurrencyListbox = document.getElementById('add-currency');

  const item = {
    transactionDate: addDateDateBox.value,
    type: addTypeListBox.value,
    recipient: addRecipientTextbox.value.trim(),
    amount: parseFloat(addAmountTextBox.value),
    currency: addCurrencyListbox.value
  };

  fetch(uri, {
    method: 'POST',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(item)
  })
    .then(response => response.json())
    .then(() => {
      getItems();
      _resetExpenseForm();
    })
    .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
  fetch(`${uri}/${id}`, {
    method: 'DELETE'
  })
  .then(() => getItems())
  .catch(error => console.error('Unable to delete item.', error));
}

function displayUpdateForm(id) {
  const item = expenses.find(item => item.id === id);

  document.getElementById('add-id').value = item.id;
  document.getElementById('add-date').value = _stripDate(item.transactionDate);
  document.getElementById('add-type').value = item.type;
  document.getElementById('add-recipient').value = item.recipient;
  document.getElementById('add-amount').value = item.amount;
  document.getElementById('add-currency').value = item.currency;
  document.getElementById('update-btn').style.visibility = 'visible';
}

function updateItem() {
  const itemId = document.getElementById('add-id').value;

  const item = {
    id: parseInt(itemId, 10),
    transactionDate: document.getElementById('add-date').value,
    type: document.getElementById('add-type').value,
    recipient: document.getElementById('add-recipient').value.trim(),
    amount: parseFloat(document.getElementById('add-amount').value),
    currency: document.getElementById('add-currency').value
  };

  fetch(`${uri}/${itemId}`, {
    method: 'PUT',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(item)
  })
    .then(() => {
      getItems();
      _resetExpenseForm();
    })
    .catch(error => console.error('Unable to update item.', error));

  closeInput();
}

function closeInput() {
  document.getElementById('update-btn').style.visibility = 'hidden';
}

function _displayCount(itemCount) {
  const name = (itemCount === 1) ? 'expense' : 'expenses';

  document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
  const tBody = document.getElementById('expenses');
  tBody.innerHTML = '';

  _displayCount(data.length);

  const button = document.createElement('button');

  data.forEach(item => {
    let editButton = button.cloneNode(false);
    editButton.innerText = 'Edit';
    editButton.setAttribute('type', 'button');
    editButton.setAttribute('class', 'btn btn-outline-dark');
    editButton.setAttribute('onclick', `displayUpdateForm(${item.id})`);

    let deleteButton = button.cloneNode(false);
    deleteButton.innerText = 'Delete';
    deleteButton.setAttribute('type', 'button');
    deleteButton.setAttribute('class', 'btn btn-danger');
    deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

    let tr = tBody.insertRow();
    let ind = 0;

    tr.insertCell(ind++)
      .appendChild(
        document.createTextNode(_stripDate(item.transactionDate))
      );
    tr.insertCell(ind++)
      .appendChild(
        document.createTextNode(item.type)
      );
    tr.insertCell(ind++)
      .appendChild(
        document.createTextNode(item.recipient)
      );
    tr.insertCell(ind++)
      .appendChild(
        document.createTextNode(item.amount)
      );
    tr.insertCell(ind++)
      .appendChild(
        document.createTextNode(item.currency)
      );
    tr.insertCell(ind++)
      .appendChild(editButton);
    tr.insertCell(ind++)
      .appendChild(deleteButton);
  });

  expenses = data;
}

function _stripDate(strDate)
{
  return strDate.substring(0, 10);
}

function _resetExpenseForm()
{
  document.getElementById('add-id').value = '';
  document.getElementById('update-btn').style.visibility = 'hidden';
}
