from email.mime import image
from unittest import result
import numpy as np
import cv2
from sklearn.cluster import KMeans
import matplotlib.pyplot as plt

image = cv2.imread("images/result.png")

# BGR -> RGB 로 변경
image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)

# height, width 통합
image = image.reshape((image.shape[0] * image.shape[1], 3)) 

# K개의 평균을 만들어 데이터를 Clustering
k = 3
clt = KMeans(n_clusters = k)
clt.fit(image)

# RGB 값을 추출할 수 있다.
# for center in clt.cluster_centers_:
#     print(center)

# 각 색깔 백분율
def centroid_histogram(clt):
    numLabels = np.arange(0, len(np.unique(clt.labels_)) + 1)
    (hist, _) = np.histogram(clt.labels_, bins = numLabels)

    hist = hist.astype("float")
    hist /= hist.sum()
    
    # 4자리 까지 받음
    return hist.round(4)

# 그래프로 그리기
def plot_colors(hist, centroids):
    bar = np.zeros((50, 300, 3), dtype="uint8")
    startX = 0

    for (percent, color) in zip(hist, centroids):
        endX = startX + (percent * 300)
        cv2.rectangle(bar, (int(startX), 0), (int(endX), 50),
                      color.astype("uint8").tolist(), -1)
        startX = endX
    return bar

# 스플래툰 결과 구하기
def splatoonResult(hist, clt):
    arr = []
    result_dict = {'Home' : 0, 'Away' : 0}
    for percent,center in zip(hist, clt.cluster_centers_):
        center = center.astype("int")
        # 기본 배경색 제거
        if(sum(center) < 10): 
            continue
        
        # 초록색에 가까우면 
        if(center[1] > 150):
            result_dict["Away"] = percent
        else:
            result_dict["Home"] = percent
    
    total = result_dict["Away"] + result_dict["Home"]

    score_Home = round(result_dict["Home"] / total * 100, 1)
    score_Away = 100 - score_Home   
    
    result_dict["Home"] = score_Home
    result_dict["Away"] = score_Away
    
    return result_dict

hist = centroid_histogram(clt)
print(hist)


splatoonResult_Dict = {}
# 스플레툰 커스텀
splatoonResult_Dict = splatoonResult(hist, clt)

# 파일 생성
savefile = open("result.txt", "w")
# 내용작성
savefile.write(str(splatoonResult_Dict))
# for team, score in splatoonResult_Dict:
    # savefile.write(f"{team} : {score} \n")
    # print(team, score)
# 작성완료
savefile.close()
print(splatoonResult_Dict)



# 결과값 그래프로 표시
bar = plot_colors(hist, clt.cluster_centers_)
plt.figure()
plt.axis("off")
plt.imshow(bar)
plt.show()