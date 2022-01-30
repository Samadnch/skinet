import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss']
})
export class TestErrorComponent implements OnInit {

  baseUrl = environment.apiUrl;
  validationErrors : any;
  constructor(private Http:HttpClient) { }

  ngOnInit(): void {
  }

  get404Error(){
    this.Http.get(this.baseUrl+'products/42').subscribe( response =>{
      console.log(response);
    }, error => {
      console.log(error)
    });
  }

  get500Error(){
    this.Http.get(this.baseUrl+'buggy/ServerError').subscribe( response =>{
      console.log(response);
    }, error => {
      console.log(error)
    });
  }

  get400Error(){
    this.Http.get(this.baseUrl+'buggy/BadRequest').subscribe( response =>{
      console.log(response);
    }, error => {
      console.log(error)
    });
  }

  get400ValidationError(){
    this.Http.get(this.baseUrl+'products/forthytwo').subscribe( response =>{
      console.log(response);
    }, error => {
      console.log(error);
      this.validationErrors = error.errors;
    });
  }

}
