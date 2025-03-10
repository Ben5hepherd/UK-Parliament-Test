import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PersonViewModel } from '../models/person-view-model';

@Injectable({
  providedIn: 'root'
})
export class PersonService {
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getById(id: number): Observable<PersonViewModel> {
    return this.http.get<PersonViewModel>(this.baseUrl + `api/person/${id}`)
  }

  getAll(): Observable<PersonViewModel[]> {
    return this.http.get<PersonViewModel[]>(this.baseUrl + 'api/person')
  }

  add(person: PersonViewModel): Observable<number> {
    return this.http.post<number>(this.baseUrl + 'api/person', person)
  }

  update(person: PersonViewModel): Observable<any> {
    return this.http.put(this.baseUrl + 'api/person', person)
  }

  delete(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + `api/person/${id}`)
  }
}
