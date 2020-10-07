

import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TokenObject, DistanceObject } from './home/home.component';

@Injectable({
  providedIn: 'root'
})
export class CoreapiService {

  constructor(private http: HttpClient) { }
 

  getToken() {
    return this.http.get<TokenObject>("https://localhost:44382/Login/GenerateToken");
  }

  getCalculateLD(token: string,firstWord:string,secondWord:string) {
    var reqHeader = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      })
    };

    return this.http.get<DistanceObject>(`https://localhost:44382/api/CalculateLD?firstWord=${firstWord}&secondWord=${secondWord}`, reqHeader);
  }
}
