import { Component, OnInit } from '@angular/core';
import { AsyncValidatorFn, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { map, of, switchMap, timer } from 'rxjs';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm! : FormGroup;
  errors : string []=[];
  constructor( private fb:FormBuilder , private accountService : AccountService , private router :Router) { }

  ngOnInit(): void {
    this.createRegisterForm();
  }

 // using formbuilder is just another way of creating form elements , we can make them also like login.component
  createRegisterForm(){

    this.registerForm = this.fb.group({

      displayName :[ null , [Validators.required]],
      email : [ null , 
                [Validators.required , Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')],
                [ this.validateEmailNotTaken()]
              ],
      password : [null , [Validators.required]]
    });
  }

  onSubmit(){ 
    if(this.registerForm?.valid) {
      this.accountService.register(this.registerForm.value).subscribe( response =>{
       this.router.navigateByUrl('/shop');
      }, error=>{
        console.log(error);
        this.errors = error.errors;
      });
    }
    
  }

  validateEmailNotTaken():AsyncValidatorFn{
    return control=>{

      return timer(500).pipe(
        switchMap(()=>{

          if(!control.value)
          {
            return of(null);
          }
          return this.accountService.checkEmailExists(control.value).pipe(
             map(res =>{
                 return res ? {emailExists:true} : null;
             })
          );

        })
      );
    };
  }


}
