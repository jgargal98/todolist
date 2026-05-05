import { Component, resource } from '@angular/core';

@Component({
  selector: 'app-root',
  standalone: true,
  template: `
    <h1>Resultado de la API:</h1>

    @if (apiResource.isLoading()) {
      <p>Cargando string...</p>
    }

    @if (apiResource.value(); as text) {
      <div class="card">
        <p>
          El servidor dice: <strong>{{ text }}</strong>
        </p>
      </div>
    }

    @if (apiResource.error()) {
      <p style="color: red;">Error de conexión</p>
    }
  `,
})
export class App {
  private readonly apiUrl = 'https://todolist-jgg.azurewebsites.net/api/';
  //Not secure, but it is what it is - azure free tier :(

  apiResource = resource({
    loader: async () => {
      const response = await fetch(this.apiUrl);
      // Usamos .text() en lugar de .json() porque recibimos un string
      return await response.text();
    },
  });
}
