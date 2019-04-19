[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SharePoint")

if((Get-PSSnapin | Where-Object {$_.Name -eq "Microsoft.SharePoint.PowerShell"}) -eq $null)
{
   Add-PSSnapIn "Microsoft.SharePoint.Powershell"
}

function migrationWeb($url){
   $web  = get-spweb $url
   $web.SiteUsers | %{ moveUser $_ }
}

function moveUser($user){
  Write-Host $user.LoginName
  if($user.LoginName.StartsWith("i:0#.w")){
    $newLoginName = $user.LoginName.Substring($user.LoginName.IndexOf("\")+1)
    Move-SPUser -Identity $user -NewAlias  "i:0#.f|fbamember|$newLoginName" -Confirm:$false -IgnoreSID -ErrorAction Continue
  }

  if($user.LoginName.StartsWith("c:0+.w")){
     moveGroup  $user
  }
}

function moveGroup($user){
  $newLoginName = $user.DisplayName.Substring($user.DisplayName.IndexOf("\")+1)
  Move-SPUser -Identity $user -NewAlias "c:0-.f|fbarole|$newLoginName" -Confirm:$false -IgnoreSID -ErrorAction Continue
}


migrationWeb http://sp