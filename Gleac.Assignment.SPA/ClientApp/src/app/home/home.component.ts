
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { CoreapiService } from '../coreapi.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  ldForm: FormGroup;
  submitted = false;
  private token: string;
  private distance: string = "0";
  private firstWordLength: string[];
  private secondWordLength: string[];
  private matrix: number[][];
  private matrixWord: number[][];
  private key: string;

  constructor(private formBuilder: FormBuilder, private ldService: CoreapiService) { }

  ngOnInit() {
    this.ldForm = this.formBuilder.group({
      firstWord: ['', [Validators.required, Validators.pattern('^[a-zA-Z ]*$'), Validators.maxLength(15)]],
      secondWord: ['', [Validators.required, Validators.pattern('^[a-zA-Z ]*$'), Validators.maxLength(15)]]
    });

    this.getToken();
  }

 
  getToken() {
    this.ldService.getToken().subscribe(
      data => {
        this.token = data.token;
      }
    );
  }  

  // convenience getter for easy access to form fields
  get f() { return this.ldForm.controls; }

  onSubmit() {
  
    this.submitted = true;

    // stop here if form is invalid
    if (this.ldForm.invalid) {
      return;
    }
    this.getKey(this.ldForm.controls['firstWord'].value, this.ldForm.controls['secondWord'].value);

    this.ldService.getCalculateLD(this.token, this.ldForm.controls['firstWord'].value, this.ldForm.controls['secondWord'].value).subscribe(
      data => {
        this.distance = data.distance;
        this.firstWordLength = data.firstWordArray;
        this.secondWordLength = data.secondWordArray;
        this.matrix = data.matrix;

        //LOCAL STORAGE 

        localStorage.setItem(this.key, this.distance);
        console.log(localStorage.getItem(this.key));
      }
    );
    
  }

  onReset() {
    this.submitted = false;
    this.distance = "0";
    this.matrix = null;
    this.ldForm.reset();
  }

  getKey(firstWord: string, secondWord: string) {
    this.key = "LD_" + firstWord + "_" + secondWord;
  }
}

//TOKEN CLASS
export class TokenObject {
  public token: string;
}

//LD CLASS
export class DistanceObject {
  public distance: string;
  public firstWordArray: string[];
  public secondWordArray: string[];
  public matrix: number[][];
}

