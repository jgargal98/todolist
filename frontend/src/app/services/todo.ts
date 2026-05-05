import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { TodoTask } from '../models/todo';

@Injectable({ providedIn: 'root' })
export class TodoService {
  private http = inject(HttpClient);

  // USA TU URL DE AZURE AQUÍ
  private apiUrl = 'https://todolist-jgg.azurewebsites.net/api/todo';

  getTodos() {
    return this.http.get<TodoTask[]>(this.apiUrl);
  }
}
