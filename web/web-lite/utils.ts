import { CodePlatform } from './code-api/common/types';

export const DEFAULT_URL = 'https://github.com/239573049/Masa.Samples';

export function parseUri(uri: string) {
  let requestUri = '';
  if (uri.startsWith('https') || uri.startsWith('http')) {
    requestUri = uri.split('://')[1] || '';
  }
  const [platform, owner, name, ...branch] = requestUri.split('/');
  const originBranch = branch.join('/').startsWith('tree') ? branch.join('/').slice(5) : '';
  return {
    uri: requestUri,
    // 固定为github
    platform: CodePlatform.github,
    owner,
    name,
    branch: originBranch,
  };
}

export function parseUris(uri: string) {
  let requestUri = '';
  if (uri.startsWith('https') || uri.startsWith('http')) {
    requestUri = uri.split('://')[1] || '';
  }
  const [platform, owner, name, ...branch] = requestUri.split('/');
  var originBranch = branch.join('/').startsWith('tree') ? branch.join('/').slice(5) : '';
  var specifiedFile;
  if(originBranch ===''){
    originBranch = branch.join('/').startsWith('blob') ? branch.join('/').slice(5) : '';
    specifiedFile =originBranch.split('/').slice(1).join('/');
  }
  return {
    uri: requestUri,
    // 固定为github
    platform: CodePlatform.github,
    owner,
    name,
    specifiedFile,
    branch: originBranch,
  };
}
