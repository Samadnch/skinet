import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { map, Observable, of } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor( private accountService : AccountService , private router : Router){}
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Promise<boolean>  {
    var result= this.accountService.loadCurrentUser(localStorage.getItem('token') as string);
 
        if (JSON.stringify(result) !== '{}' ) {
          return  Promise.resolve(true);
        }
        return this.router.navigate(['account/login'], { queryParams: { returnUrl: state.url } });
   
  }
  
}
