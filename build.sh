#!/bin/bash
set -e
echo "BUILD:"

function clearDockerLibrary {
	echo "Clear docker library"
	if [[ $(docker ps -a | grep -v CONTAINER | wc -l) -gt 0 ]]; then
		docker rm -f $(docker ps -a | awk '{print $1}' | grep -v CONTAINER)
	fi
	rm -rf /var/lib/docker/tmp/*
	imagesToClear=`docker images -q`;
		
	if [[ ${#imagesToClear[@]} -eq 0 || ${#imagesToClear[@]} -eq 1 && -z "$imagesToClear" ]] ; then 
		echo "No images to purge (debug count: ${#imagesToClear[@]}):'${imagesToClear[@]}'";
	else
		echo "Images to purge (count: ${#imagesToClear[@]}): '${imagesToClear[@]}'"
		docker rmi -f `docker images -q`
	fi
}

function checkBranchMode {
	echo "Check branch mode"
	
	branchVersion=''
	
	if [[ $Branch == *feature* ]]; then
		branchVersion="-"${Branch#*feature/}
		branchMode=1
	fi;
	
	if [[ $Branch == *pull-requests* ]]; then
		branchVersion="-"${Branch#*pull-requests/}
		branchMode=1
	fi;
	
	if (( branchMode == 0 )); then
		echo "Application build in push mode"
		buildImageId="$DockerInternalFeed/$buildImageId$branchVersion"
		outputImageId="$DockerInternalFeed/$outputImageId"
	else
		echo "Application build branch mode"
		buildImageId="dci/$buildImageId$branchVersion"
		outputImageId="dci/$outputImageId"
	fi
	
	echo $branchMode > branch_mode
	echo $buildImageId > build_image_tag
	echo $outputImageId > output_image_id
	
	echo "branch version: $branchVersion"
	echo "branch mode: $branchMode"
	echo "build image# $buildImageId"
	echo "output image# $outputImageId"
}

function changePropVersion {
	sed -i "s/\(<$1>\).*\(\(<\/$1>\)\)/\1$2\2/" Directory.Build.props
}

codeVersion=`date +"%Y.%m.%d.%H"`
version="$Version.$Timestamp"
echo $version > version
commitId=$BUILD_VCS_NUMBER
echo $commitId > commit_id

changePropVersion "Version" $codeVersion
changePropVersion "FileVersion" $codeVersion
changePropVersion "InformationalVersion" $ReleaseNumber"+"$commitId

cat Directory.Build.props

echo "Release number: $ReleaseNumber"
echo "Version: $version"
echo "Commit hash#: $commitId"
echo "Parameters count: $#"
echo "Build number: $BuildNumber"
echo "Branch name: $Branch"
echo "Docker repository path: $DockerInternalFeed"
echo "NuGet Internal Feed: $NuGetInternalFeed"
echo "NuGet External Feed: $NuGetExternalFeed"

imageName="tokenizerapi";
echo $imageName > output_image_name
buildImageName="tokenizer-build";
outputImageId="$imageName:$version"
buildImageId="$buildImageName:$version"
branchMode=0

clearDockerLibrary || true
checkBranchMode

echo "Building containers ..."

# build image
docker build --network host . --tag $buildImageId --build-arg Branch=$Branch --build-arg docker_feed=$DockerInternalFeed --build-arg nuget_feed_external=$NuGetExternalFeed --build-arg nuget_feed_internal=$NuGetInternalFeed

# output image
docker build --network host ./src/Api --tag $outputImageId --build-arg docker_feed=$DockerInternalFeed --build-arg build_image=$buildImageId

echo "Building containers ... done."