import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="flex flex-col items-center justify-center min-h-screen bg-gray-100 p-4">
      <div class="bg-white p-8 rounded-lg shadow-md w-full max-w-md">
        <h1 class="text-2xl font-bold text-blue-600 mb-4 text-center">Prueba de Conexión</h1>

        @if (loading()) {
          <div class="flex justify-center">
            <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
          </div>
        } @else if (error()) {
          <div class="bg-red-100 text-red-700 p-3 rounded border border-red-400">
            <strong>Error:</strong> {{ error() }}
          </div>
        } @else {
          <div class="bg-green-100 text-green-800 p-4 rounded border border-green-400">
            <p class="text-sm uppercase font-semibold">Respuesta del servidor:</p>
            <p class="text-lg italic">"{{ data() }}"</p>
          </div>
        }

        <button
          (click)="fetchData()"
          class="mt-6 w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 transition"
        >
          Reintentar conexión
        </button>
      </div>
    </div>
  `,
})
export class AppComponent implements OnInit {
  // Signals para manejar el estado de forma limpia
  data = signal<string>('');
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  private readonly apiUrl = 'https://todolist-jgg.azurewebsites.net/api/';

  ngOnInit() {
    this.fetchData();
  }

  async fetchData() {
    this.loading.set(true);
    this.error.set(null);

    try {
      const response = await fetch(this.apiUrl);

      if (!response.ok) {
        throw new Error(`Error del servidor: ${response.status}`);
      }

      const text = await response.text();
      this.data.set(text);
    } catch (err: any) {
      this.error.set(err.message || 'No se pudo conectar con el backend');
    } finally {
      this.loading.set(false);
    }
  }
}
